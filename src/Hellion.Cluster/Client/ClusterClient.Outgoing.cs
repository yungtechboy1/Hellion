using Hellion.Core.Data.Headers;
using Hellion.Core.Network;
using Hellion.Database.Structures;
using System.Collections.Generic;
using System.Linq;

namespace Hellion.Cluster.Client
{
    public partial class ClusterClient
    {
        /// <summary>
        /// Sends the cluster client session Id.
        /// </summary>
        private void SendSessionId()
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.WELCOME);
                packet.Write((int)this.sessionId);

                this.Send(packet);
            }
        }

        /// <summary>
        /// Send the pong request to the client.
        /// </summary>
        /// <param name="time"></param>
        private void SendPong(int time)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.PING);
                packet.Write(time);

                this.Send(packet);
            }
        }

        /// <summary>
        /// Sends a cluster error to the client.
        /// </summary>
        /// <param name="code"></param>
        private void SendClusterError(ErrorType code)
        {
            this.SendClusterMessage((int)code);
        }

        /// <summary>
        /// Sends a cluster message to the client with the specific code.
        /// </summary>
        /// <param name="code"></param>
        private void SendClusterMessage(int code)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.ERROR);
                packet.Write(code);

                this.Send(packet);
            }
        }

        private void SendWorldIp(string ip)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.CACHE_ADDR);
                packet.Write(ip);

                this.Send(packet);
            }
        }

        /// <summary>
        /// Send the login num pad to the client
        /// </summary>
        private void SendLoginNumPad()
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.LOGIN_PROTECT_NUMPAD);
                packet.Write(this.loginProtectValue);

                this.Send(packet);
            }
        }

        /// <summary>
        /// Send a new login num pad to the client.
        /// </summary>
        private void SendLoginProtect()
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.LOGIN_PROTECT_CERT);
                packet.Write(0);
                packet.Write(this.loginProtectValue);

                this.Send(packet);
            }
        }

        /// <summary>
        /// Send the character list to the client.
        /// </summary>
        /// <param name="authKey">authentication key</param>
        /// <param name="characters"></param>
        private void SendCharacterList(int authKey, IEnumerable<DbCharacter> characters)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.PLAYER_LIST);
                packet.Write(authKey);

                if (characters.Any())
                {
                    packet.Write(characters.Count());
                    foreach (var character in characters)
                    {
                        packet.Write(character.Slot);
                        packet.Write(1); // this number represents the selected character in the window
                        packet.Write(character.MapId);
                        packet.Write(0x0B + character.Gender); // Model id
                        packet.Write(character.Name);
                        packet.Write(character.PosX);
                        packet.Write(character.PosY);
                        packet.Write(character.PosZ);
                        packet.Write(character.Id);
                        packet.Write(0); // Party id
                        packet.Write(0); // Guild id
                        packet.Write(0); // War Id
                        packet.Write(character.SkinSetId); // SkinSet Id
                        packet.Write(character.HairId);
                        packet.Write(character.HairColor);
                        packet.Write(character.FaceId);
                        packet.Write(character.Gender);
                        packet.Write(character.ClassId);
                        packet.Write(character.Level);
                        packet.Write(0); // Job Level (Maybe master of hero ?)
                        packet.Write(character.Strength);
                        packet.Write(character.Stamina);
                        packet.Write(character.Dexterity);
                        packet.Write(character.Intelligence);
                        packet.Write(0); // Mode ??
                        packet.Write(character.Items.Count(i => i.ItemSlot > 42));

                        foreach (var item in character.Items.Where(i => i.ItemSlot > 42))
                            packet.Write(item.ItemId);
                    }

                    // Messenger?
                    packet.Write(0);
                }
                else
                    packet.Write<long>(0);

                this.Send(packet);
            }
        }

        /// <summary>
        /// Send the packet that indicates the client can open the world.
        /// </summary>
        public void SendJoinWorld()
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.PRE_JOIN);

                this.Send(packet);
            }
        }
    }
}
