﻿using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Riven.Update.OrbwalkingEvents
{
    internal class Jungle
    {
        public static void OnProcessAutoAttack(Obj_AI_Minion mob)
        {
            if (mob == null || mob.MaxHealth <= 7 || MenuConfig.Jungle["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(1500) >= 1)
            {
                return;
            }

            if (SpellConfig.Q.Ready && MenuConfig.Jungle["Q"].Enabled)
            {
                SpellManager.CastQ(mob);
            }

            if (MenuConfig.Jungle["W"].Enabled && SpellConfig.W.Ready && Extensions.CurrentQCount <= 1 && !SpellConfig.Q.Ready)
            {
                SpellManager.CastW(mob);
            }

            if (SpellConfig.E.Ready && MenuConfig.Jungle["E"].Enabled && Global.Player.Level <= 4)
            {
                SpellConfig.E.Cast(mob.ServerPosition);
            }
        }

        public static void OnUpdate()
        {

            if (SpellConfig.E.Ready && MenuConfig.Jungle["E"].Enabled && Global.Player.Level > 4)
            {
                var mob = GameObjects.Jungle.FirstOrDefault(x => x.IsValidTarget() && x.MaxHealth > 10 && x.Distance(Global.Player) <= Extensions.EngageRange);
                if (mob != null)
                {
                    SpellConfig.E.Cast(mob.ServerPosition);
                }
            }

            if (SpellConfig.Q.Ready)
            {
                var legendary = GameObjects.JungleLegendary.FirstOrDefault(x => x.Health < Global.Player.GetSpellDamage(x, SpellSlot.Q));
                if (legendary == null)
                {
                    return;
                }

                SpellManager.CastQ(legendary);
            }
        }
    }
}
