using Ether.Network.Packets;
using Hellion.Core.Database;
using Hellion.Core.IO;
using Hellion.Core.Network;
using Hellion.World.Structures;
using Hellion.World.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hellion.Core.Extensions;

namespace Hellion.World.Modules
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
                this.items[i] = new Item();

            if (dbItems != null && dbItems.Count > 0)
            {
                foreach (var item in dbItems)
                    this.items[item.ItemSlot] = new Item(item);
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
        public bool Move(int sourceSlot, int destSlot)
        {
            if (sourceSlot == destSlot || sourceSlot >= MaxItems || destSlot >= MaxItems)
                return false;
            if (this.items[sourceSlot].Id < 0 || this.items[sourceSlot].UniqueId < 0)
                return false;
            
            this.items[sourceSlot].Slot = destSlot;

            if (this.items[destSlot].Slot != -1)
                this.items[destSlot].Slot = sourceSlot;

            this.items.Swap(sourceSlot, destSlot);

            return true;
        }

        public void Equip(Item item)
        {
            Log.Debug("Equip(item)");
            int destSlot = item.Data.Parts + EquipOffset;
            int sourceSlot = item.Slot;

            if (this.items[destSlot].Id != -1)
                this.Unequip(this.items[destSlot]);

            this.items[sourceSlot].Slot = destSlot;
            this.items.Swap(sourceSlot, destSlot);
            
            this.player.SendItemEquip(item, item.Data.Parts, true);
        }

        public void Unequip(Item item)
        {
            
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
    }
}
