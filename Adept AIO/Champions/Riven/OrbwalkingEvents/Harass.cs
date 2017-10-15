﻿namespace Adept_AIO.Champions.Riven.OrbwalkingEvents
{
    using System.Linq;
    using System.Threading;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using Core;
    using Miscellaneous;
    using SDK.Unit_Extensions;

    public class Harass
    {
        // Goes for CC heavy & (or) ranged enemies
        private static readonly string[] Dangerous =
        {
            "Darius",
            "Garen",
            "Galio",
            "Kled",
            "Malphite",
            "Maokai",
            "Trundle",
            "Swain",
            "Tahm Kench",
            "Ryze",
            "Shen",
            "Singed",
            "Poppy",
            "Pantheon",
            "Nasus",
            "Renekton",
            "Quinn"
        };

        // Melee's who are weak with CC
        private static readonly string[] Melee = {"Fiora", "Irelia", "Akali", "Udyr", "Rengar", "Jarvan IV"};

        // Mostly fighters
        private static readonly string[] SemiCombo = {"Yasuo", "LeeSin", "XinZhao", "Aatrox"};

        public static void OnProcessAutoAttack()
        {
            var target = GameObjects.EnemyHeroes.OrderBy(x => x.Distance(Global.Player)).FirstOrDefault();
            if (target == null || !MenuConfig.Harass[target.ChampionName].Enabled)
            {
                return;
            }

            switch (Enums.Current)
            {
                case HarassPattern.SemiCombo:
                    if (SpellConfig.Q.Ready)
                    {
                        SpellManager.CastQ(target);
                    }
                    break;
                case HarassPattern.AvoidTarget:
                    if (SpellConfig.Q.Ready && Extensions.CurrentQCount == 2)
                    {
                        SpellManager.CastQ(target);
                    }
                    break;
                case HarassPattern.BackToTarget:
                    if (SpellConfig.Q.Ready && Extensions.CurrentQCount == 2)
                    {
                        SpellManager.CastQ(target);
                    }
                    break;
            }
        }

        public static void OnUpdate()
        {
            Enums.Current = Generate();

            var target = Global.TargetSelector.GetTarget(Extensions.EngageRange);

            if (target == null)
            {
                return;
            }

            if (!MenuConfig.Harass[target.ChampionName].Enabled)
            {
                return;
            }

            var antiPosition = GetDashPosition(target);

            switch (Enums.Current)
            {
                case HarassPattern.SemiCombo:

                    #region SemiCombo

                    if (!SpellConfig.Q.Ready &&
                        SpellConfig.E.Ready &&
                        Extensions.CurrentQCount == 1 &&
                        !Global.Orbwalker.CanAttack() &&
                        Global.Orbwalker.CanMove())
                    {
                        SpellConfig.E.Cast(antiPosition);
                        SpellConfig.W.Cast();
                    }

                    #endregion

                    break;
                case HarassPattern.AvoidTarget:

                    #region Away

                    if (SpellConfig.Q.Ready && Extensions.CurrentQCount == 1)
                    {
                        SpellManager.CastQ(target, true);
                    }

                    if (SpellConfig.W.Ready)
                    {
                        SpellManager.CastW(target);
                    }

                    if (SpellConfig.Q.Ready &&
                        SpellConfig.E.Ready &&
                        Extensions.CurrentQCount == 3 &&
                        !Global.Orbwalker.CanAttack())
                    {
                        SpellConfig.E.Cast(antiPosition);
                        DelayAction.Queue(190, () => SpellConfig.Q.Cast(antiPosition), new CancellationToken(false));
                    }

                    #endregion

                    break;
                case HarassPattern.BackToTarget:

                    #region Target

                    if (SpellConfig.Q.Ready && Extensions.CurrentQCount == 1)
                    {
                        SpellManager.CastQ(target, true);
                    }

                    if (SpellConfig.W.Ready)
                    {
                        SpellManager.CastW(target);
                    }

                    if (SpellConfig.Q.Ready &&
                        SpellConfig.E.Ready &&
                        Extensions.CurrentQCount == 3 &&
                        !Global.Orbwalker.CanAttack())
                    {
                        SpellConfig.E.Cast(antiPosition);
                        DelayAction.Queue(190, () => SpellConfig.Q.Cast(target), new CancellationToken(false));
                    }

                    #endregion

                    break;
            }
        }

        private static Vector3 GetDashPosition(Obj_AI_Base target)
        {
            switch (MenuConfig.Harass["Dodge"].Value)
            {
                case 0:
                    var turret = GameObjects.AllyTurrets.Where(x => x.IsValid).
                        OrderBy(x => x.Distance(Global.Player)).
                        FirstOrDefault();
                    return turret != null ? turret.ServerPosition : Game.CursorPos;
                case 1: return Game.CursorPos;

                case 2:
                    return Global.Player.ServerPosition +
                           (Global.Player.ServerPosition - target.ServerPosition).Normalized() * 300;
            }
            return Vector3.Zero;
        }

        // Prob going to need a rework
        private static HarassPattern Generate()
        {
            var target = Global.TargetSelector.GetTarget(Extensions.EngageRange + 700);
            if (target == null)
            {
                return HarassPattern.SemiCombo;
            }

            switch (MenuConfig.Harass["Mode"].Value)
            {
                case 0:
                    if (target.IsUnderEnemyTurret() || Dangerous.Contains(target.ChampionName))
                    {
                        return HarassPattern.AvoidTarget;
                    }

                    if (Melee.Contains(target.ChampionName))
                    {
                        return HarassPattern.BackToTarget;
                    }
                    return SemiCombo.Contains(target.ChampionName)
                        ? HarassPattern.SemiCombo
                        : HarassPattern.AvoidTarget;

                case 1: return HarassPattern.SemiCombo;
                case 2: return HarassPattern.AvoidTarget;
                case 3: return HarassPattern.BackToTarget;
            }
            return HarassPattern.SemiCombo;
        }
    }
}