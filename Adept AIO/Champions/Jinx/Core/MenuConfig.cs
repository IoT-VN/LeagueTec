﻿using System.Collections.Generic;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Menu_Extension;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Jinx.Core
{
    internal class MenuConfig
    {
        public Menu Whitelist, 
                    Combo,
                    Harass,
                    LaneClear, 
                    JungleClear,
                    Killsteal,
                    Drawings;

        public void Attach()
        {
            var mainMenu = new Menu(string.Empty, $"Adept AIO - {Global.Player.ChampionName}", true);
            mainMenu.Attach();

            Global.Orbwalker.Attach(mainMenu);

            Gapcloser.Attach(mainMenu, "Anti Gapcloser");

            Whitelist = new Menu("Whitelist", "Whitelist");
            foreach (var hero in GameObjects.EnemyHeroes)
            {
                Whitelist.Add(new MenuBool(hero.ChampionName, "Use R Against: " + hero.ChampionName));
            }

            Combo = new Menu("Combo", "Combo")
            {
                new MenuBool("Q", "Automatic Q"),
                new MenuSliderBool("W", "Use W If Distance <", true, 1350, 0, 1500),
                new MenuBool("Close", "Use E If Enemy Is Close"),
                new MenuBool("Count", "Use E As AoE & Close Targets"),
                new MenuBool("Teleport", "Use E On Teleport"),
                new MenuBool("Immovable", "Use E At Immovable Targets"),
                new MenuKeyBind("Semi", "Semi R Key", KeyCode.T, KeybindType.Press),
            };

            Harass = new Menu("Harass", "Harass")
            {
                new MenuBool("Q", "Automatic Q"),
                new MenuSliderBool("W", "Use W If Distance <", true, 1300, 0, 1500)
            };

            LaneClear = new Menu("LaneClear", "LaneClear")
            {
                new MenuBool("Q", "Automatic Q"),
                new MenuBool("W", "Use W At Big Minions", false)
            };

            JungleClear = new Menu("JungleClear", "JungleClear")
            {
                new MenuBool("Q", "Automatic Q"),
                new MenuSliderBool("W", "Use W (Min. Mana %)", true, 65)
            };

            Killsteal = new Menu("Killsteal", "Killsteal")
            {
                new MenuSliderBool("Range", "[R] Maximum Range", true, 1500, 500, 5000),
                new MenuBool("W", "Use W")
            };

            Drawings = new Menu("Drawings", "Drawings")
            {
                new MenuSlider("Segments", "Segments", 100, 100, 200).SetToolTip("Smoothness of the circles"),
                new MenuBool("Dmg", "Damage"),
                new MenuBool("R", "R Auto Range"),
                new MenuBool("W", "W Range")
            };

            foreach (var menu in new List<Menu>
            {
                Whitelist,
                Combo,
                Harass,
                LaneClear,
                JungleClear,
                Killsteal,
                Drawings,
                MenuShortcut.Credits
            })  mainMenu.Add(menu);
        }
    }
}
