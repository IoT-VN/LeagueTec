﻿using System;
using System.Linq;
using Adept_AIO.SDK.Extensions;
using Aimtec;

namespace Adept_AIO.SDK.Usables
{
    internal class Items
    {
        private static readonly string[] Tiamats = {"ItemTiamatCleave", "ItemTitanicHydraCleave", "ItemTiamatCleave"};
        public static float TiamatCastTime;
        public static void CastTiamat()
        {
            SpellSlot? slot = null;
          
            foreach (var tiamat in Tiamats)
            {
                if (CanUseItem(tiamat))
                {
                    slot = GetItemSlot(tiamat);
                }
            }
            
            if (slot != null)
            {
                GlobalExtension.Player.SpellBook.CastSpell((SpellSlot) slot);
                TiamatCastTime = Environment.TickCount;
            }
        }

        public static void CastItem(string itemName, Vector3 position = new Vector3())
        {
            var slot = GetItemSlot(itemName);
            if (!CanUseItem(itemName))
            {
                return;
            }

            if (position == Vector3.Zero)
            {
                GlobalExtension.Player.SpellBook.CastSpell(slot);
            }
            else
            {
                GlobalExtension.Player.SpellBook.CastSpell(slot, position);
            }
        }

        public static bool CanUseItem(string itemName)
        {
            var slot = GetItemSlot(itemName);
        
            if (slot != SpellSlot.Unknown)
            {
                return GlobalExtension.Player.SpellBook.GetSpellState(slot) == SpellState.Ready;
            }
            return false;
        }

        private static SpellSlot GetItemSlot(string itemName)
        {
            var slot = GlobalExtension.Player.Inventory.Slots.FirstOrDefault(x => string.Equals(itemName, x.SpellName, StringComparison.CurrentCultureIgnoreCase));
           
            if (slot != null && slot.SpellSlot != SpellSlot.Unknown)
            {
                return slot.SpellSlot;
            }
            return SpellSlot.Unknown;
        }
    }
}
