using Ether.Network.Packets;
using Hellion.Core.Data.Headers;
using Hellion.Core.Network;

/*
 * This file contains only the incoming packets realated with the inventory.
 * Move item, drop item, sell item, etc...
 */

namespace Hellion.World.Client
{
    public partial class WorldClient
    {
        [FFIncomingPacket(PacketType.MOVEITEM)]
        private void OnItemMoveInInventory(NetPacketBase packet)
        {
            var itemType = packet.Read<byte>();
            var sourceSlot = packet.Read<byte>();
            var destSlot = packet.Read<byte>();

            this.Player.Inventory.Move(sourceSlot, destSlot);
        }

        [FFIncomingPacket(PacketType.DOEQUIP)]
        private void OnItemUnequip(NetPacketBase packet)
        {
            var itemUniqueId = packet.Read<int>();
            var item = this.Player.Inventory.GetItemByUniqueId(itemUniqueId);

            if (item == null)
                return;

            this.Player.Inventory.Unequip(item);
        }

        [FFIncomingPacket(PacketType.DOUSEITEM)]
        private void OnItemUsage(NetPacketBase packet)
        {
            var itemUniqueId = (packet.Read<int>() >> 16) & 0xFFFF;
            var objectId = packet.Read<int>();
            var equipPart = packet.Read<int>();

            var item = this.Player.Inventory.GetItemByUniqueId(itemUniqueId);

            if (item != null && item.Id > 0)
            {
                if (item.Data.Parts > 0)
                    this.Player.Inventory.Equip(item);
                //else
                //    this.Player.Inventory.UseItem(item);
            }
        }
    }
}
