﻿using Adept_AIO.Champions.Rengar.Core;
using Adept_AIO.Champions.Rengar.Drawings;
using Adept_AIO.Champions.Rengar.Miscellaneous;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;

namespace Adept_AIO.Champions.Rengar
{
    internal class Rengar
    {
        public static void Init()
        {
            MenuConfig.Attach();
            SpellConfig.Load();

            Game.OnUpdate += Manager.OnUpdate;
            Global.Orbwalker.PostAttack += Manager.PostAttack;
            Render.OnRender += DrawManager.OnRender;
            Render.OnPresent += DrawManager.RenderDamage;
        }
    }
}
