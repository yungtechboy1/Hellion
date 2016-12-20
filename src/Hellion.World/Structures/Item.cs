using Ether.Network.Packets;
using Hellion.Core.Database;

namespace Hellion.World.Structures
{
    /// <summary>
    /// FlyFF item structure.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Gets the item Id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the creator id of the item.
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// Gets the item quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets the item current slot.
        /// </summary>
        public int Slot { get; set; }

        /// <summary>
        /// Creates an empty item.
        /// </summary>
        /// <remarks>
        /// All values set to -1.
        /// </remarks>
        public Item()
            : this(-1, -1, -1, -1)
        {
        }

        /// <summary>
        /// Create and initialize an item from a <see cref="DbItem"/> from the database context.
        /// </summary>
        /// <param name="item">Item from database</param>
        public Item(DbItem item)
            : this(item.ItemId, item.ItemCount, item.CreatorId, item.ItemSlot)
        {
        }

        /// <summary>
        /// Create an item with an id.
        /// </summary>
        /// <param name="id">Item Id</param>
        public Item(int id)
            : this(id, 1, -1, -1)
        {
        }

        /// <summary>
        /// Create an item with an id and a quantity.
        /// </summary>
        /// <param name="id">Item id</param>
        /// <param name="quantity">Item quantity</param>
        public Item(int id, int quantity)
            : this(id, quantity, -1, -1)
        {
        }

        /// <summary>
        /// Create an item with an id, quantity and creator id.
        /// </summary>
        /// <param name="id">Item id</param>
        /// <param name="quantity">Item quantity</param>
        /// <param name="creatorId">Id of the character that created the object (for GM)</param>
        public Item(int id, int quantity, int creatorId)
            : this(id, quantity, creatorId, -1)
        {
        }

        /// <summary>
        /// Create an item with an id, quantity, creator id and destination slot.
        /// </summary>
        /// <param name="id">Item id</param>
        /// <param name="quantity">Itme quantity</param>
        /// <param name="creatorId">Id of the character that created the object (for GM)</param>
        /// <param name="slot">Item slot</param>
        public Item(int id, int quantity, int creatorId, int slot)
        {
            this.Id = id;
            this.Quantity = quantity;
            this.CreatorId = creatorId;
            this.Slot = slot;
        }

        /// <summary>
        /// Serialize the item into the packet.
        /// </summary>
        /// <param name="packet"></param>
        public void Serialize(NetPacketBase packet)
        {
            packet.Write(this.Id);
            packet.Write(0);
            packet.Write(0);
            packet.Write((short)this.Quantity);
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
}
