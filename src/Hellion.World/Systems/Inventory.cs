using Ether.Network.Packets;
using Hellion.Core.IO;
using Hellion.World.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using Hellion.Core.Extensions;
using Hellion.Database.Structures;

namespace Hellion.World.Systems
{
    public class Inventory
    {
        public const int EquipOffset = 42;
        public const int MaxItems = 73;
        public const int InventorySize = EquipOffset;
        public const int MaxHumanParts = MaxItems - EquipOffset;

        private Item[] items;
        private Player player;

        /// <summary>
        /// Creates and initialize the inventory from the items stored in database.
        /// </summary>
        /// <param name="owner">Inventory owner</param>
        /// <param name="dbItems">Items in database</param>
        public Inventory(Player owner, ICollection<DbItem> dbItems)
        {
            this.player = owner;
            this.items = new Item[MaxItems];

            for (int i = 0; i < MaxItems; ++i)
            {
                this.items[i] = new Item();
                this.items[i].UniqueId = i;
            }

            if (dbItems != null && dbItems.Count > 0)
            {
                foreach (var item in dbItems)
                    this.items[item.ItemSlot] = new Item(item);
            }

            for (int i = EquipOffset; i < MaxItems; ++i)
            {
                if (this.items[i].Id == -1)
                    this.items[i].UniqueId = -1;
            }
        }

        /// <summary>
        /// Gets the equiped item list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Item> GetEquipedItems()
        {
            return this.items.ToList().GetRange(EquipOffset, MaxItems - EquipOffset);
        }

        /// <summary>
        /// Get an item by a slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public Item GetItemBySlot(int slot)
        {
            return this.items.Where(i => i.Slot == slot).FirstOrDefault();
        }

        /// <summary>
        /// Gets an item by his unique id.
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        public Item GetItemByUniqueId(int uniqueId)
        {
            return this.items.Where(i => i.UniqueId == uniqueId).FirstOrDefault();
        }

        /// <summary>
        /// Gets the index of an available slot.
        /// </summary>
        /// <returns></returns>
        public int GetFreeSlot()
        {
            for (int i = 0; i < EquipOffset; ++i)
            {
                if (this.items[i] != null && this.items[i].Id == -1)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Move an item in the inventory.
        /// </summary>
        /// <param name="sourceSlot"></param>
        /// <param name="destSlot"></param>
        /// <returns></returns>
        public void Move(int sourceSlot, int destSlot)
        {
            if (sourceSlot < 0 || sourceSlot >= MaxItems || destSlot < 0 || destSlot >= MaxItems)
                return;
            if (this.items[sourceSlot].Id == -1 || this.items[sourceSlot].UniqueId == -1 || this.items[destSlot].UniqueId == -1)
                return;
            
            var sourceItem = this.items[sourceSlot];
            var destItem = this.items[destSlot];

            bool stackable = sourceItem.Id == destItem.Id && sourceItem.Data.PackMax > 1;

            if (stackable)
            {
                // TODO: stack items
            }
            else
            {
                sourceItem.Slot = destSlot;
                if (destItem.Slot != -1)
                    destItem.Slot = sourceSlot;

                this.items.Swap(sourceSlot, destSlot);

                this.player.SendItemMove((byte)sourceSlot, (byte)destSlot);
            }
        }

        public void Equip(Item item)
        {
            // TODO: Add some verifications before equip.
            // Sex, level, job, ride, etc...

            int equipParts = item.Data.Parts;
            int destSlot = equipParts + EquipOffset;
            int sourceSlot = item.Slot;

            if (this.items[destSlot].Id != -1)
                this.Unequip(this.items[destSlot]);

            item.Slot = destSlot;
            this.items.Swap(sourceSlot, destSlot);

            this.player.SendItemEquip(item, equipParts, true);
        }

        public void Unequip(Item item)
        {
            int sourceSlot = item.Slot;
            int destSlot = this.GetFreeSlot();

            // TODO: add more verifications
            if (destSlot == -1)
            {
                Log.Error("No more space in inventory");
                return;
            }

            if (item != null && item.Id > 0 && item.Slot < EquipOffset)
            {
                this.Equip(item);
                return;
            }
            if (item.Id > 0 && item.Slot > EquipOffset)
            {
                int parts = Math.Abs(sourceSlot - EquipOffset);

                item.Slot = destSlot;
                this.items.Swap(sourceSlot, destSlot);

                this.player.SendItemEquip(item, parts, false);
            }
        }

        /// <summary>
        /// Serialize the inventory.
        /// </summary>
        /// <param name="packet"></param>
        public void Serialize(NetPacketBase packet)
        {
            for (int i = 0; i < MaxItems; ++i)
                packet.Write(this.items[i].UniqueId);

            packet.Write((byte)this.items.Count(x => x.Id != -1));

            for (int i = 0; i < MaxItems; ++i)
            {
                if (this.items[i].Id > 0)
                {
                    packet.Write((byte)this.items[i].UniqueId);
                    packet.Write(this.items[i].UniqueId);
                    this.items[i].Serialize(packet);
                }
            }

            for (int i = 0; i < MaxItems; ++i)
                packet.Write(this.items[i].UniqueId);
        }

        /// <summary>
        /// Saves the inventory.
        /// </summary>
        public void Save()
        {
        }
    }
}
