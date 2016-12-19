using Hellion.Core.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Structures
{
    public class Item
    {
        public int Id { get; private set; }

        public int CreatorId { get; set; }

        public int Quantity { get; set; }

        public int Slot { get; set; }

        public Item()
            : this(-1, -1, -1, -1)
        {
        }

        public Item(DbItem item)
            : this(item.ItemId, item.ItemCount, item.CreatorId, item.ItemSlot)
        {
        }

        public Item(int id)
            : this(id, 1, -1, -1)
        {
        }

        public Item(int id, int quantity)
            : this(id, quantity, -1, -1)
        {
        }

        public Item(int id, int quantity, int creatorId)
            : this(id, quantity, creatorId, -1)
        {
        }

        public Item(int id, int quantity, int creatorId, int slot)
        {
            this.Id = id;
            this.Quantity = quantity;
            this.CreatorId = creatorId;
            this.Slot = slot;
        }
    }
}
