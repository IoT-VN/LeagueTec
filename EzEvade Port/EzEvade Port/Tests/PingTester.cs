﻿namespace EzEvade_Port.Tests
{
    using System;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Helpers;
    using Utils;

    class PingTester
    {
        public static Menu testMenu;

        private static float lastTimerCheck = 0;
        private static bool lastRandomMoveCoeff;

        private static float sumPingTime;
        private static float averagePingTime = ObjectCache.GamePing;
        private static int testCount;
        private static int autoTestCount;
        private static float maxPingTime = ObjectCache.GamePing;

        private static bool autoTestPing;

        private static EvadeCommand lastTestMoveToCommand;

        public PingTester()
        {
            Game.OnUpdate += Game_OnGameUpdate;

            testMenu = new Menu("PingTest", "Ping Tester", true);
            testMenu.Add(new MenuBool("AutoSetPing", "Auto Set Ping"));
            testMenu.Add(new MenuBool("TestMoveTime", "Test Ping"));
            testMenu.Add(new MenuBool("SetMaxPing", "Set Max Ping"));
            testMenu.Add(new MenuBool("SetAvgPing", "Set Avg Ping"));
            testMenu.Add(new MenuBool("Test20MoveTime", "Test Ping x20"));
            testMenu.Add(new MenuBool("PrintResults", "Print Results"));
            testMenu.Attach();
        }

        private static Obj_AI_Hero myHero => ObjectManager.GetLocalPlayer();

        private void IssueTestMove(int recursionCount)
        {
            var movePos = ObjectCache.MyHeroCache.ServerPos2D;

            var rand = new Random();

            lastRandomMoveCoeff = !lastRandomMoveCoeff;
            if (lastRandomMoveCoeff)
            {
                movePos.X += 65 + rand.Next(0, 20);
            }
            else
            {
                movePos.X -= 65 + rand.Next(0, 20);
            }

            lastTestMoveToCommand = new EvadeCommand {Order = EvadeOrderCommand.MoveTo, TargetPosition = movePos, Timestamp = Environment.TickCount, IsProcessed = false};
            myHero.IssueOrder(OrderType.MoveTo, movePos.To3D());

            if (recursionCount > 1)
            {
                DelayAction.Add(500, () => IssueTestMove(recursionCount - 1));
            }
        }

        private void SetPing(int ping)
        {
            ObjectCache.MenuCache.Cache["ExtraPingBuffer"].As<MenuSlider>().Value = ping;
        }

        private void Game_OnGameUpdate()
        {
            if (testMenu["AutoSetPing"].Enabled)
            {
                Console.WriteLine("Testing Ping...Please wait 10 seconds");

                var testAmount = 20;

                testMenu["AutoSetPing"].As<MenuBool>().Value = false;
                IssueTestMove(testAmount);
                autoTestCount = testCount + testAmount;
                autoTestPing = true;
            }

            if (testMenu["PrintResults"].Enabled)
            {
                testMenu["PrintResults"].As<MenuBool>().Value = false;

                Console.WriteLine("Average Extra Delay: " + averagePingTime);
                Console.WriteLine("Max Extra Delay: " + maxPingTime);
            }

            if (autoTestPing && testCount >= autoTestCount)
            {
                Console.WriteLine("Auto Set Ping Complete");

                Console.WriteLine("Average Extra Delay: " + averagePingTime);
                Console.WriteLine("Max Extra Delay: " + maxPingTime);

                SetPing((int) (averagePingTime + 10));
                Console.WriteLine("Set Average extra ping + 10: " + (averagePingTime + 10));

                autoTestPing = false;
            }

            if (testMenu["TestMoveTime"].Enabled)
            {
                testMenu["TestMoveTime"].As<MenuBool>().Value = false;
                IssueTestMove(1);
            }

            if (testMenu["Test20MoveTime"].Enabled)
            {
                testMenu["Test20MoveTime"].As<MenuBool>().Value = false;
                IssueTestMove(20);
            }

            if (testMenu["SetMaxPing"].Enabled)
            {
                testMenu["SetMaxPing"].As<MenuBool>().Value = false;

                if (testCount < 10)
                {
                    Console.WriteLine("Please test 10 times before setting ping");
                }
                else
                {
                    Console.WriteLine("Set Max extra ping: " + maxPingTime);
                    SetPing((int) maxPingTime);
                }
            }

            if (testMenu["SetAvgPing"].Enabled)
            {
                testMenu["SetAvgPing"].As<MenuBool>().Value = false;

                if (testCount < 10)
                {
                    Console.WriteLine("Please test 10 times before setting ping");
                }
                else
                {
                    Console.WriteLine("Set Average extra ping: " + averagePingTime);
                    SetPing((int) averagePingTime);
                }
            }

            if (myHero.HasPath)
            {
                if (lastTestMoveToCommand != null && lastTestMoveToCommand.IsProcessed == false && lastTestMoveToCommand.Order == EvadeOrderCommand.MoveTo)
                {
                    var path = myHero.Path;

                    if (path.Length > 0)
                    {
                        var movePos = path[path.Length - 1].To2D();

                        if (movePos.Distance(lastTestMoveToCommand.TargetPosition) < 10)
                        {
                            var moveTime = Environment.TickCount - lastTestMoveToCommand.Timestamp - ObjectCache.GamePing;
                            Console.WriteLine("Extra Delay: " + moveTime);
                            lastTestMoveToCommand.IsProcessed = true;

                            sumPingTime += moveTime;
                            testCount += 1;
                            averagePingTime = sumPingTime / testCount;
                            maxPingTime = Math.Max(maxPingTime, moveTime);
                        }
                    }
                }
            }
        }
    }
}