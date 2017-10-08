﻿using Adept_BaseUlt.Local_SDK;
using Adept_BaseUlt.Manager;
using Aimtec.SDK.Events;

namespace Adept_BaseUlt
{
    internal class Program
    {
        private static void Main()
        {
            GameEvents.GameStart += delegate
            {
                switch (Global.Player.ChampionName)
                {
                    case "Ashe":
                        new BaseUlt(1600, 130, 250, 1);
                        break;
                    case "Draven":
                        new BaseUlt(2000, 160, 300);
                        break;
                    case "Ezreal":
                        new BaseUlt(2000, 160, 1000);
                        break;
                    case "Jinx":
                        new BaseUlt(2200, 140, 500, 1);
                        break;
                    case "Karthus":
                        new BaseUlt(int.MaxValue, int.MaxValue, 3000);
                        break;
                    case "Ziggs":
                        new BaseUlt(1750, 275, 250, int.MaxValue, 5250);
                        break;
                }
            };
        }
    }
}
