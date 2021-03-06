﻿namespace Adept_AIO.Champions.Gragas.Core
{
    using Aimtec;
    using Aimtec.SDK.Damage;
    using SDK.Unit_Extensions;

    class Dmg
    {
        public static double Damage(Obj_AI_Hero target)
        {
            if (target == null)
            {
                return 0;
            }

            var dmg = Global.Player.GetAutoAttackDamage(target);

            if (SpellManager.Q.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.Q);
            }

            if (SpellManager.W.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.W);
            }

            if (SpellManager.E.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.E);
            }

            if (SpellManager.R.Ready)
            {
                dmg += Global.Player.GetSpellDamage(target, SpellSlot.R);
            }
            return dmg;
        }
    }
}