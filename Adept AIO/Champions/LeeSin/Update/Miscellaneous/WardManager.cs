﻿using System;
using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.LeeSin.Update.Miscellaneous
{
    internal class WardManager
    {
        public static float LastWardCreated;
        public static Vector3 WardPosition;
     
        public static bool IsWardReady => WardNames.Any(Items.CanUseItem) && Environment.TickCount - LastWardCreated > 1500 || LastWardCreated == 0;

        private static readonly string[] WardNames =
        {
            "TrinketTotemLvl1",
            "ItemGhostWard",
            "JammerDevice",
        };

        public static void JumpToVector(Vector3 position)
        {
            var bestobject = GetBestObject(position);

            if (bestobject != null)
            {
                SpellConfig.W.CastOnUnit(bestobject);
            }
        }

        public static void WardJump(Vector3 position, bool maxRange)
        {
            if (Environment.TickCount - LastWardCreated < 1500 && LastWardCreated > 0)
            {
                return;
            }

            var ward = WardNames.FirstOrDefault(Items.CanUseItem);
            if (ward == null)
            {
                return;
            }

            if (maxRange)
            {
                position = GlobalExtension.Player.ServerPosition.Extend(position, 600);
            }

            Items.CastItem(ward, position);
            LastWardCreated = Environment.TickCount;
            WardPosition = position;
        }

        public static Obj_AI_Minion GetBestObject(Vector3 position, bool allowMinions = true, bool atWall = false)
        {
            if (atWall)
            {
                return GameObjects.AllyWards.FirstOrDefault();
            }

            var wards = GameObjects.AllyWards.Where(x => x.IsValid).OrderBy(x => x.Distance(position)).FirstOrDefault(x => x.Distance(position) <= 600 && GlobalExtension.Player.Distance(x) <= 600 && x.Distance(WardPosition) <= 10);

            if (wards != null)
            {
                return wards;
            }

            var minions = GameObjects.EnemyMinions.Where(x => GlobalExtension.Player.Distance(x) <= SpellConfig.W.Range && x.Distance(position) <= 400)
                .OrderBy(x => x.Distance(position))
                .FirstOrDefault();

            return allowMinions ? minions : null;
        }

        public static void OnCreate(GameObject sender)
        {
            var ward = sender as Obj_AI_Minion;
         
            if (ward == null || WardPosition.Distance(ward.Position) > 700 || Environment.TickCount - LastWardCreated > 1000 || !Extension.IsFirst(SpellConfig.W))
            {
                return;
            }

            WardPosition = ward.Position;
            GlobalExtension.Player.SpellBook.CastSpell(SpellSlot.W, ward.Position);
        }
    }
}
