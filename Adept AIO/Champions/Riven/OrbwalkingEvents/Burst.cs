﻿namespace Adept_AIO.Champions.Riven.OrbwalkingEvents
{
    using System;
    using System.Threading;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using Core;
    using Miscellaneous;
    using SDK.Generic;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    class Burst
    {
        public static void OnProcessAutoAttack()
        {
            var target = Global.TargetSelector.GetTarget(Global.Player.AttackRange + 200);
            if (target == null)
            {
                return;
            }

            if (SpellConfig.W.Ready && SpellConfig.Q.Ready)
            {
                SpellManager.CastWq(target);
            }

            else if (SpellConfig.Q.Ready)
            {
                SpellManager.CastQ(target);
            }
        }

        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetSelectedTarget();

            if (target == null || !MenuConfig.BurstMenu[target.ChampionName].Enabled)
            {
                return;
            }

            Enums.BurstPattern = GeneratePattern(target);

            Extensions.AllIn = SummonerSpells.IsValid(SummonerSpells.Flash);

            if (!target.IsValidTarget(Extensions.FlashRange()))
            {
                return;
            }

            if (SpellConfig.R2.Ready && Enums.UltimateMode == UltimateMode.Second && (Extensions.CurrentQCount == 3 || !SpellConfig.Q.Ready))
            {
                SpellManager.CastR2(target);
            }

            if (SpellConfig.R.Ready && Enums.UltimateMode == UltimateMode.First && SpellConfig.E.Ready)
            {
                SpellConfig.E.Cast(target.ServerPosition);
                SpellConfig.R.Cast();
            }
            else if (SpellConfig.E.Ready)
            {
                SpellConfig.E.Cast(target.ServerPosition);
            }

            switch (Enums.BurstPattern)
            {
                case BurstPattern.TheShy:

                    if (Extensions.AllIn && target.Distance(Global.Player) > SpellConfig.E.Range + Global.Player.AttackRange && SummonerSpells.IsValid(SummonerSpells.Flash))
                    {
                        if (Items.CanUseTiamat())
                        {
                            DelayAction.Queue(300,
                                delegate
                                {
                                    Items.CastTiamat();
                                    SummonerSpells.Flash.Cast(target.ServerPosition);
                                },
                                new CancellationToken(false));
                        }
                        else
                        {
                            DelayAction.Queue(200,
                                delegate
                                {
                                    SummonerSpells.Flash.Cast(target.ServerPosition);
                                },
                                new CancellationToken(false));
                        }
                    }
                    break;

                case BurstPattern.Execution:

                    if (SpellConfig.R.Ready && Enums.UltimateMode == UltimateMode.First)
                    {
                        SpellConfig.R.Cast();
                    }
                    else if (SpellConfig.E.Ready && Enums.UltimateMode == UltimateMode.Second && Game.TickCount - SpellConfig.R.LastCastAttemptT >= 1100)
                    {
                        SpellConfig.E.Cast(target.ServerPosition);
                        SpellConfig.R2.Cast(target.ServerPosition);

                        DelayAction.Queue(100,
                            () =>
                            {
                                SummonerSpells.Flash.Cast(target.ServerPosition);
                            },
                            new CancellationToken(false));

                        DelayAction.Queue(500,
                            () =>
                            {
                                SpellConfig.W.Cast();
                                SpellManager.CastQ(target);
                            },
                            new CancellationToken(false));
                    }

                    break;
            }
        }

        private static BurstPattern GeneratePattern(Obj_AI_Base target)
        {
            switch (MenuConfig.BurstMenu["Mode"].Value)
            {
                case 0: return Maths.Percent(target.Health, Dmg.Damage(target)) > 135 ? BurstPattern.Execution : BurstPattern.TheShy;
                case 1: return BurstPattern.TheShy;
                case 2: return BurstPattern.Execution;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}