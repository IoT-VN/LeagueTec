﻿namespace Adept_AIO.Champions.Zoe.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Generic;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        public static void OnUpdate()
        {
            var creep = GameObjects.Jungle.OrderBy(x => x.Distance(Global.Player)).ThenBy(x => x.MaxHealth).FirstOrDefault(x => x.IsValidTarget(SpellManager.Q.Range + 2000) && x.MaxHealth > 15);
            if (creep == null )
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.JungleClear["Q"].Enabled)
            {
                if (SpellManager.R.Ready &&
                    MenuConfig.JungleClear["R"].Enabled)
                {
                    //SpellManager.CastR(creep);
                }

                SpellManager.CastQ(creep);
            }
            
            if (SpellManager.E.Ready && MenuConfig.JungleClear["E"].Enabled)
            {
                SpellManager.CastE(creep);
            }
        }
    }
}