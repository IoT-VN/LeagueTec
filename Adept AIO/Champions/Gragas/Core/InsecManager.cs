﻿namespace Adept_AIO.Champions.Gragas.Core
{
    using System;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using LeeSin.Core;
    using SDK.Unit_Extensions;

    class InsecManager
    {
        public static Vector3 QInsecPos(Obj_AI_Base target)
        {
            var insecPos = InsecPosition(target);
            var finalPos = GetTargetEndPosition();
            var qPos = insecPos + (insecPos - finalPos).Normalized() * -(SpellManager.KnockBackRange + DistanceBehindTarget(target));
            return qPos;
        }

        private static float DistanceBehindTarget(GameObject target = null)
        {
            return Math.Min((Global.Player.BoundingRadius + (target == null ? 65 : target.BoundingRadius) + 50) * 1.25f, SpellManager.R.Range);
        }

        public static Vector3 InsecPosition(Obj_AI_Base target)
        {
            var pos = target.ServerPosition + (target.ServerPosition - GetTargetEndPosition()).Normalized() * DistanceBehindTarget(target);

            return NavMesh.WorldToCell(pos).Flags.HasFlag(NavCellFlags.Wall) ? Vector3.Zero : pos;
        }

        public static Vector3 GetTargetEndPosition()
        {
            if (SpellManager.Barrel != null)
            {
                return SpellManager.Barrel.Center.To3D();
            }

            var ally = GameObjects.AllyHeroes.FirstOrDefault(x => x.Distance(Global.Player) <= 1500);
            var turret = GameObjects.AllyTurrets.OrderBy(x => x.Distance(Global.Player)).FirstOrDefault();

            if (turret != null)
            {
                Temp.IsAlly = false;
                return turret.ServerPosition;
            }
            if (ally == null)
            {
                return Vector3.Zero;
            }
            Temp.IsAlly = true;
            return ally.ServerPosition;
        }
    }
}