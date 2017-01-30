using Ether.Network.Packets;
using Hellion.Core.Data.Headers;
using Hellion.Core.IO;
using Hellion.Core.Network;
using Hellion.Core.Structures;
using Hellion.Database;
using Hellion.Database.Structures;
using Hellion.World.Systems;

/*
 * This file contains only the incoming packets realated with the Player.
 * Packets as the moves, duels, chat, etc...
 */

namespace Hellion.World.Client
{
    public partial class WorldClient
    {
        /// <summary>
        /// The client sended the Join request to join the world as a player.
        /// </summary>
        /// <param name="packet"></param>
        [FFIncomingPacket(PacketType.JOIN)]
        private void OnJoin(NetPacketBase packet)
        {
            var worldId = packet.Read<int>();
            var playerId = packet.Read<int>();
            var authKey = packet.Read<int>();
            var partyId = packet.Read<int>();
            var guildId = packet.Read<int>();
            var guildWarId = packet.Read<int>();
            var idOfMulti = packet.Read<int>(); // what is this?
            var slot = packet.Read<byte>();
            var playerName = packet.Read<string>();
            var username = packet.Read<string>();
            var password = packet.Read<string>();
            var messengerState = packet.Read<int>();
            var messengerCount = packet.Read<int>();
            // Not using messenger yet

            this.CurrentUser = DatabaseService.Users.Get(x =>
                x.Username.ToLower() == username.ToLower() &&
                x.Password.ToLower() == password.ToLower() &&
                x.Authority > 0);

            if (this.CurrentUser == null)
            {
                Log.Warning("Unknow account: '{0}'.", username);
                this.Server.RemoveClient(this);
                return;
            }

            DbCharacter character = DatabaseService.Characters.Get(x =>
                x.AccountId == this.CurrentUser.Id &&
                x.Name.ToLower() == playerName.ToLower() &&
                x.Id == playerId, includes => includes.Items); // TODO: include more

            if (character == null)
            {
                Log.Warning("Cannot find character '{0}' with id {1} for account '{2}'.", playerName, playerId, this.CurrentUser.Id);
                this.Server.RemoveClient(this);
                return;
            }

            this.Player = new Player(this, character);
            this.Player.SendPlayerSpawn();

            Map playerMap = WorldServer.MapManager[this.Player.MapId];

            if (playerMap == null)
            {
                Log.Error("Invalid MapId: {0}", this.Player.MapId);
                this.Server.RemoveClient(this);
                return;
            }

            playerMap.AddObject(this.Player);
        }

        /// <summary>
        /// The client has send a snapshot request.
        /// Which means that this is a packet containing several packets.
        /// </summary>
        /// <param name="packet"></param>
        [FFIncomingPacket(PacketType.SNAPSHOT)]
        private void OnSnapshot(NetPacketBase packet)
        {
            var snapshotCount = packet.Read<byte>();

            while (snapshotCount != 0)
            {
                var snapshotHeaderNumber = packet.Read<ushort>();
                var snapshotHeader = (SnapshotType)snapshotHeaderNumber;

                Log.Debug("Recieve snapshot: {0}", snapshotHeader);

                switch (snapshotHeader)
                {
                    case SnapshotType.DESTPOS:
                        var posX = packet.Read<float>();
                        var posY = packet.Read<float>();
                        var posZ = packet.Read<float>();
                        var forward = packet.Read<byte>();

                        this.Player.MovingFlags = ObjectState.OBJSTA_NONE;
                        this.Player.MovingFlags |= ObjectState.OBJSTA_FMOVE;
                        this.Player.DestinationPosition = new Vector3(posX, posY, posZ);
                        this.Player.Angle = Vector3.AngleBetween(this.Player.Position, this.Player.DestinationPosition);
                        this.Player.SendMoverMoving();
                        break;
                    default: FFPacket.UnknowPacket<SnapshotType>(snapshotHeaderNumber, 4); break;
                };

                snapshotCount--;
            }
        }

        /// <summary>
        /// The client is moving with the keyboard.
        /// </summary>
        /// <param name="packet"></param>
        [FFIncomingPacket(PacketType.PLAYERMOVED)]
        private void OnMoveByKeyboard(NetPacketBase packet)
        {
            var startPositionX = packet.Read<float>();
            var startPositionY = packet.Read<float>();
            var startPositionZ = packet.Read<float>();

            this.Player.Position = new Vector3(startPositionX, startPositionY, startPositionZ);

            var directionX = packet.Read<float>();
            var directionY = packet.Read<float>();
            var directionZ = packet.Read<float>();

            var directionVector = new Vector3(directionX, directionY, directionZ);

            this.Player.Position += directionVector;
            this.Player.Angle = packet.Read<float>();

            this.Player.MovingFlags = (ObjectState)packet.Read<uint>();
            this.Player.MotionFlags = (StateFlags)packet.Read<int>();
            this.Player.ActionFlags = packet.Read<int>();
            var motionEx = packet.Read<int>();

            var loop = packet.Read<int>();
            var motionOption = packet.Read<int>();

            var tick = packet.Read<long>();

            int delay = (int)((Time.GetTickFrom(tick) / 10000) / 66.6667f);

            // TODO: Checks

            //this.Player.SendMoveByKeyboard(directionVector, motionEx, loop, motionOption, tick);
        }
    }
}
