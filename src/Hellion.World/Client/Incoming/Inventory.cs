using Hellion.Core.Network;
using Hellion.Core.Data.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ether.Network.Packets;
using Hellion.Core.IO;

/*
 * This file contains only the incoming packets realated with the inventory.
 * Move item, drop item, sell item, etc...
 */

namespace Hellion.World.Client
{
    public partial class WorldClient
    {
        [FFIncomingPacket(WorldHeaders.Incoming.ItemMoveInIntentory)]
        private void OnItemMoveInInventory(NetPacketBase packet)
        {
            var itemType = packet.Read<byte>();
            var sourceSlot = packet.Read<byte>();
            var destSlot = packet.Read<byte>();

            Log.Debug("Moving item from {0} to {1}", sourceSlot, destSlot);

            if (this.Player.Inventory.Move(sourceSlot, destSlot))
                this.Player.SendItemMove(sourceSlot, destSlot);
        }

        [FFIncomingPacket(WorldHeaders.Incoming.ItemUnequip)]
        private void OnItemUnequip(NetPacketBase packet)
        {
            var itemUniqueId = packet.Read<int>();
            var item = this.Player.Inventory.GetItemByUniqueId(itemUniqueId);

            if (item == null)
                return;

            this.Player.Inventory.Unequip(item);
        }

        [FFIncomingPacket(WorldHeaders.Incoming.ItemUsage)]
        private void OnItemUsage(NetPacketBase packet)
        {
            var itemUniqueId = (packet.Read<int>() >> 16) & 0xFFFF;
            var objectId = packet.Read<int>();
            var equipPart = packet.Read<int>();

            Log.Debug("OnItemUsage: itemUniqueId: {0} ; objectId: {1} ; equipPart: {2}", itemUniqueId, objectId, equipPart);

            if (equipPart >= Modules.Inventory.MaxHumanParts)
                return;

            var item = this.Player.Inventory.GetItemByUniqueId(itemUniqueId);

            if (item?.Data.Parts > 0)
                this.Player.Inventory.Equip(item);
            else
            {
                // Use items
            }
        }
    }
}
