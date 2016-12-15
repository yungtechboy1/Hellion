using Ether.Network;
using Hellion.Core.Database;
using Hellion.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Ether.Network.Packets;
using Hellion.Core;
using Hellion.Core.Data.Headers;
using Hellion.Core.Network;
using Hellion.World.Structures;

namespace Hellion.World
{
    public partial class WorldClient : NetConnection
    {
        private uint sessionId;

        private DbUser currentUser;

        /// <summary>
        /// Gets or sets the current player.
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// Gets or sets the WorldServer reference.
        /// </summary>
        public WorldServer Server { get; set; }

        /// <summary>
        /// Creates a new WorldClient instance.
        /// </summary>
        public WorldClient()
            : base()
        {
            this.sessionId = (uint)Global.GenerateRandomNumber();
        }

        /// <summary>
        /// Creates a new WorldClient instance.
        /// </summary>
        /// <param name="socket">Client socket</param>
        public WorldClient(Socket socket)
            : base(socket)
        {
            this.sessionId = (uint)Global.GenerateRandomNumber();
        }

        public void Disconnected()
        {
            Log.Info("Client with id {0} disconnected.", this.Id);

            this.Dispose();
            
            WorldServer.MapManager[this.Player.MapId].RemoveObject(this.Player);
            this.Player.SpawnedObjects.Clear();
        }

        /// <summary>
        /// Send hi to the client.
        /// </summary>
        public override void Greetings()
        {
            var packet = new FFPacket();

            packet.Write(0);
            packet.Write((int)this.sessionId);

            this.Send(packet);
        }

        /// <summary>
        /// Handle incoming packets.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public override void HandleMessage(NetPacketBase packet)
        {
            packet.Position = 17;

            var packetHeaderNumber = packet.Read<uint>();
            var packetHeader = (WorldHeaders.Incoming)packetHeaderNumber;

            Log.Debug("Recieve packet: {0}", packetHeader);

            if (FFPacketHandler.Invoke(this, packetHeader, packet) == false)
                FFPacket.UnknowPacket<WorldHeaders.Incoming>(packetHeaderNumber, 8);

            // Old code...
            //switch (packetHeader)
            //{
            //    case WorldHeaders.Incoming.Join: this.OnJoin(packet); break;
            //    case WorldHeaders.Incoming.MoveByMouse: this.OnMoveByMouse(packet); break;

            //    default: FFPacket.UnknowPacket<WorldHeaders.Incoming>(packetHeaderNumber, 8); break;
            //}

            base.HandleMessage(packet);
        }
    }
}
