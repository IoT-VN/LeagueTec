﻿using System.Linq;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents.LaneClear
{
    internal class LaneClear : ILaneClear
    {
        public bool Q1Enabled { get; set; }
        public bool WEnabled { get; set; }
        public bool EEnabled { get; set; }
        public bool CheckEnabled { get; set; }

        private readonly ISpellConfig _spellConfig;

        public LaneClear(ISpellConfig spellConfig)
        {
            _spellConfig = spellConfig;
        }

        public void OnPostAttack()
        {
            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.Distance(Global.Player) < Global.Player.AttackRange + x.BoundingRadius &&
                                                                      x.Health > Global.Player.GetAutoAttackDamage(x));

            if (minion == null || !CheckEnabled && Global.Player.CountEnemyHeroesInRange(2000) >= 1)
            {
                return;
            }

            if (_spellConfig.E.Ready && EEnabled && minion.Health < Global.Player.GetSpellDamage(minion, SpellSlot.E))
            {
                if (Items.CanUseTiamat())
                {
                    Items.CastTiamat(false);
                    DelayAction.Queue(50, () => _spellConfig.E.Cast(minion));
                }
                else
                {
                    _spellConfig.E.Cast(minion);
                }
            }
            else if (_spellConfig.W.Ready && WEnabled)
            {
                _spellConfig.W.CastOnUnit(Global.Player);
            }
        }

        public void OnUpdate()
        {
            if (_spellConfig.Q.Ready && Q1Enabled || Global.Orbwalker.IsWindingUp || CheckEnabled && Global.Player.CountEnemyHeroesInRange(2000) >= 1)
            {
                return;
            }

            var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.Distance(Global.Player) < (Global.Player.IsUnderEnemyTurret() ? _spellConfig.Q.Range : _spellConfig.Q.Range / 2f));

            if (minion == null)
            {
                return;
            }

            _spellConfig.Q.Cast(minion);
        }
    }
}
