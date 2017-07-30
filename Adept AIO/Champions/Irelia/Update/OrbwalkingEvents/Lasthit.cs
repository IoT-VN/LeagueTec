﻿using System.Linq;
using Adept_AIO.Champions.Irelia.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Irelia.Update.OrbwalkingEvents
{
    internal class Lasthit
    {
        public static void OnUpdate()
        {
            if (!SpellConfig.Q.Ready || !MenuConfig.Clear["Lasthit"].Enabled || MenuConfig.Clear["Lasthit"].Value > GlobalExtension.Player.ManaPercent())
            {
                return;
            }

            foreach (var minion in GameObjects.EnemyMinions.Where(x => x.Health < GlobalExtension.Player.GetSpellDamage(x, SpellSlot.Q) && 
                                                                       x.Distance(GlobalExtension.Player) < SpellConfig.Q.Range))
            {
                if (!minion.IsValid || minion.Distance(GlobalExtension.Player) < GlobalExtension.Player.AttackRange || MenuConfig.Clear["Turret"].Enabled && minion.IsUnderEnemyTurret())
                {
                    continue;
                }

                SpellConfig.Q.CastOnUnit(minion);
            }
        }
    }
}
