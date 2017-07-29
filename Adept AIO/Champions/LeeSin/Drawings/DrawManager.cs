﻿using System;
using System.Drawing;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Drawings
{
    internal class DrawManager
    {
        public static void RenderManager()
        {
            if (GlobalExtension.Player.IsDead)
            {
                return;
            }

            if (MenuConfig.Drawings["Q"].Enabled && SpellConfig.Q.Ready)
            {
                Render.Circle(GlobalExtension.Player.Position, SpellConfig.Q.Range, (uint)MenuConfig.Drawings["Segments"].Value, Color.IndianRed);
            }

            if (MenuConfig.Drawings["Position"].Enabled && Extension.InsecPosition != Vector3.Zero)
            {
                Render.Circle(Extension.InsecPosition, 65, (uint)MenuConfig.Drawings["Segments"].Value, Color.White);
            }
        }
    }
}
