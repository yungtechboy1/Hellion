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
    }
}
