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
                    
                    // Item structure
                    packet.Write(this.items[i].Id);
                    packet.Write(0);
                    packet.Write(0);
                    packet.Write((short)this.items[i].Quantity);
                    packet.Write<byte>(0);
                    packet.Write(0);
                    packet.Write(0);
                    packet.Write<byte>(0);
                    packet.Write(0);
                    packet.Write(0);
                    packet.Write<byte>(0);
                    packet.Write(0);
                    packet.Write(0);
                    packet.Write(0);
                    packet.Write(0);
                    packet.Write(0);
                    packet.Write<long>(0);
                    packet.Write<long>(0);
                    packet.Write<byte>(0);
                    packet.Write(0);
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
