using Hellion.Core.Data.Headers;
using Hellion.Core.ISC.Structures;
using Hellion.Core.Network;

namespace Hellion.Login.Client
{
    public partial class LoginClient
    {
        /// <summary>
        /// Sends a login error to the client.
        /// </summary>
        /// <param name="code"></param>
        private void SendLoginError(ErrorType code)
        {
            this.SendLoginErrorMessage((int)code);
        }

        /// <summary>
        /// Sends a login message to the client with the specific code.
        /// </summary>
        /// <param name="code"></param>
        private void SendLoginErrorMessage(int code)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.ERROR);
                packet.Write(code);

                this.Send(packet);
            }
        }

        /// <summary>
        /// Sends the available servers to the client.
        /// </summary>
        private void SendServerList()
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.SRVR_LIST);
                packet.Write(0);
                packet.Write<byte>(1);
                packet.Write(this.username);
                packet.Write(this.GetServerCount());

                foreach (ClusterServerInfo cluster in LoginServer.Clusters)
                {
                    packet.Write(-1);
                    packet.Write(cluster.Id);
                    packet.Write(cluster.Name);
                    packet.Write(cluster.Ip);
                    packet.Write(0);
                    packet.Write(0);
                    packet.Write(1);
                    packet.Write(0);

                    foreach (WorldServerInfo world in cluster.Worlds)
                    {
                        packet.Write(cluster.Id);
                        packet.Write(world.Id);
                        packet.Write(world.Name);
                        packet.Write(world.Ip);
                        packet.Write(0);
                        packet.Write(0);
                        packet.Write(1);
                        packet.Write(world.Capacity);
                    }
                }

                this.Send(packet);
            }
        }
    }
}
