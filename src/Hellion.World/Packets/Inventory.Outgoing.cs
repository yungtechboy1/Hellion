using Hellion.Core.Data.Headers;
using Hellion.Core.Network;
using Hellion.World.Structures;
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

        internal void SendItemEquip(Item item, int targetSlot, bool equip)
        {
            var packet = new FFPacket();

            packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.ItemChangeEquipState);
            packet.Write(item.UniqueId);
            packet.Write<byte>(0);
            packet.Write(equip ? (byte)0x01 : (byte)0x00);
            packet.Write(item.Id);
            packet.Write<short>(0); // Refine
            packet.Write<byte>(0); // element
            packet.Write<byte>(0); // element refine
            packet.Write(0);
            packet.Write(targetSlot);

            this.SendToVisible(packet);
        }
    }
}
