using Ether.Network.Packets;
using Hellion.Core.Database;
using Hellion.World.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Modules
{
    public class Inventory
    {
        public const int EquipOffset = 42;
        public const int MaxItems = 73;

        private Item[] items;

        /// <summary>
        /// Creates and initialize the inventory from the items stored in database.
        /// </summary>
        /// <param name="dbItems">Items in database</param>
        public Inventory(ICollection<DbItem> dbItems)
        {
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
        /// Serialize the inventory.
        /// </summary>
        /// <param name="packet"></param>
        public void Serialize(NetPacketBase packet)
        {
            for (int i = 0; i < MaxItems; ++i)
            {
                if (this.items[i].Id != -1)
                    packet.Write(i);
                else
                    packet.Write(-1);
            }
            
            packet.Write((byte)this.items.Count(x => x.Id != -1));

            for (int i = 0; i < MaxItems; ++i)
            {
                if (this.items[i].Id > 0)
                {
                    packet.Write((byte)i);
                    packet.Write(i);
                    this.items[i].Serialize(packet);
                }
            }

            for (int i = 0; i < MaxItems; ++i)
            {
                if (this.items[i].Id != -1)
                    packet.Write(i);
                else
                    packet.Write(-1);
            }
        }

    }
}
