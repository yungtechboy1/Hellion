using Ether.Network.Packets;
using Hellion.Core.Data.Headers;
using Hellion.Core.Network;

/*
 * This file contains only the incoming packets realated with the chat.
 * Normal chat, shout and GM commands handler.
 */

namespace Hellion.World.Client
{
    public partial class WorldClient
    {
        [FFIncomingPacket(PacketType.CHAT)]
        private void OnChat(NetPacketBase packet)
        {
            var chatMessage = packet.Read<string>();

            if (string.IsNullOrEmpty(chatMessage))
                return;

            if (chatMessage.StartsWith("/"))
                this.Player.Chat.CommandChat(chatMessage);
            else
                this.Player.SendNormalChat(chatMessage);
        }
    }
}
