﻿using System.Drawing;
using System.Linq;
using Adept_AIO.Champions.Jinx.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Damage;

namespace Adept_AIO.Champions.Jinx.Drawings
{
    internal class DrawManager
    {
        private readonly SpellConfig _spellConfig;
        private readonly MenuConfig _menuConfig;
        private readonly Dmg _dmg;

        public DrawManager(MenuConfig menuConfig, Dmg dmg, SpellConfig spellConfig)
        {
            _menuConfig = menuConfig;
            _dmg = dmg;
            _spellConfig = spellConfig;
        }

        public void OnPresent()
        {
            if (Global.Player.IsDead || !_menuConfig.Drawings["Dmg"].Enabled)
            {
                return;
            }

            foreach (var target in GameObjects.EnemyHeroes.Where(x => !x.IsDead && x.IsFloatingHealthBarActive && x.IsVisible))
            {
                var damage = Global.Player.GetSpellDamage(target, SpellSlot.R);

                Global.DamageIndicator.Unit = target;
                Global.DamageIndicator.DrawDmg((float)damage, Color.FromArgb(153, 12, 177, 28));
            }
        }

        public void OnRender()
        {
            if (Global.Player.IsDead)
            {
                return;
            }

            if (_menuConfig.Drawings["R"].Enabled)
            {
                Render.Circle(Global.Player.Position, _menuConfig.Killsteal["Range"].Value, (uint)_menuConfig.Drawings["Segments"].Value, Color.CadetBlue);
            }

            if (_menuConfig.Drawings["W"].Enabled)
            {
                Render.Circle(Global.Player.Position, _spellConfig.W.Range, (uint)_menuConfig.Drawings["Segments"].Value, Color.Gray);
            }
        }
    }
}
