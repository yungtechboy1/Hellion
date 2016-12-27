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
        internal void SendNormalChat(string message)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.MoverChat);
                packet.Write(message);

                this.SendToVisible(packet);
            }
        }
    }
}
