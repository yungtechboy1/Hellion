using Hellion.Core.Data.Headers;
using Hellion.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Systems
{
    public partial class Player
    {
        internal void SendItemMove(byte sourceSlot, byte destinationSlot)
        {
            var packet = new FFPacket();

            packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.ItemMoveInInventory);
            packet.Write<byte>(0);
            packet.Write(sourceSlot);
            packet.Write(destinationSlot);

            this.Send(packet);
        }
    }
}
