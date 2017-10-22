﻿namespace Adept_AIO.Champions.Kayn.OrbwalkingEvents
{
    using System.Threading;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using Core;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    class Combo
    {
        public static void OnPostAttack(AttackableUnit target)
        {
            if (target == null)
            {
                return;
            }

            Items.CastTiamat();

            if (target.HealthPercent() >= 40 || !MenuConfig.Combo["R"].Enabled || !SpellConfig.R.Ready)
            {
                return;
            }

            SpellConfig.R.CastOnUnit(target);
        }

        public static void OnUpdate()
        {
            if (SpellConfig.E.Ready && MenuConfig.Combo["E"].Enabled)
            {
                var end = Global.Player.Position.Extend(Game.CursorPos, 200);
                var point = NavMesh.WorldToCell(end).Flags == NavCellFlags.Wall;

                if (point)
                {
                    SpellConfig.E.Cast();
                }
            }

            var target = Global.TargetSelector.GetTarget(SpellConfig.R.Range);
            if (target == null)
            {
                return;
            }

            if (SpellConfig.W.Ready && MenuConfig.Combo["W"].Enabled && target.IsValidTarget(SpellConfig.W.Range))
            {
                SpellConfig.W.Cast(target);
            }
            else if (SpellConfig.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
                if (target.IsValidTarget(SpellConfig.W.Range))
                {
                    Global.Player.SpellBook.CastSpell(SpellSlot.Q, target.ServerPosition);
                    DelayAction.Queue(500, () => Items.CastTiamat(), new CancellationToken(false));
                }
                else if (SpellConfig.R.Ready &&
                         MenuConfig.Combo["Beyblade"].Enabled &&
                         SummonerSpells.IsValid(SummonerSpells.Flash) &&
                         target.Distance(Global.Player) > SpellConfig.Q.Range &&
                         Dmg.Damage(target) * 1.5f >= target.Health)
                {
                    SummonerSpells.Flash.Cast(target.ServerPosition);
                    Global.Player.SpellBook.CastSpell(SpellSlot.Q, target.ServerPosition);
                }
            }

            if (SpellConfig.R.Ready &&
                MenuConfig.Combo["R"].Enabled &&
                (MenuConfig.Combo["R"].Value >= Global.Player.HealthPercent() ||
                 MenuConfig.Combo["R"].Value >= target.HealthPercent() ||
                 Dmg.Damage(target) * 1.5 > target.Health))
            {
                if (MenuConfig.Whitelist[target.ChampionName].Enabled)
                {
                    SpellConfig.R.CastOnUnit(target);
                }
            }
        }
    }
}