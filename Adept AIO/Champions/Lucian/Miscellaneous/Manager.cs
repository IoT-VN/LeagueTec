﻿namespace Adept_AIO.Champions.Lucian.Miscellaneous
{
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using OrbwalkingEvents;
    using SDK.Unit_Extensions;

    class Manager
    {
        public static void OnUpdate()
        {
            Global.Orbwalker.AttackingEnabled = !Global.Player.HasBuff("LucianR");

            if (Global.Player.IsDead || Global.Orbwalker.IsWindingUp)
            {
                return;
            }

            switch (Global.Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo.OnUpdate();
                    break;
                case OrbwalkingMode.Mixed:
                    Harass.OnUpdate();
                    break;
                case OrbwalkingMode.Laneclear:
                    LaneClear.OnUpdate();
                    JungleClear.OnUpdate();
                    break;
            }
        }

        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            switch (Global.Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo.PostAttack(sender, args);
                    break;
                case OrbwalkingMode.Mixed:
                    Harass.PostAttack(sender, args);
                    break;
                case OrbwalkingMode.Laneclear:
                    LaneClear.PostAttack(sender, args);
                    JungleClear.PostAttack(sender, args);
                    break;
            }
        }
    }
}