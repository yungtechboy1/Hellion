using Ether.Network.Packets;
using Hellion.Core.Database;
using Hellion.Core.IO;
using Hellion.Core.Network;
using Hellion.World.Structures;
using Hellion.World.Systems.Map;

namespace Hellion.World
{
    public partial class WorldClient
    {
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

            this.currentUser = DatabaseService.Users.Get(x =>
                x.Username.ToLower() == username.ToLower() &&
                x.Password.ToLower() == password.ToLower() &&
                x.Authority > 0);

            if (this.currentUser == null)
            {
                Log.Warning("Unknow account: '{0}'.", username);
                this.Server.RemoveClient(this);
                return;
            }

            DbCharacter character = DatabaseService.Characters.Get(x =>
                x.AccountId == this.currentUser.Id &&
                x.Name.ToLower() == playerName.ToLower() &&
                x.Id == playerId, includes => includes.Items); // TODO: include more

            if (character == null)
            {
                Log.Warning("Cannot find character '{0}' with id {1} for account '{2}'.", playerName, playerId, this.currentUser.Id);
                this.Server.RemoveClient(this);
                return;
            }

            this.Player = new Player(this, character);
            this.SendPlayerSpawn();

            Map playerMap = WorldServer.MapManager[this.Player.MapId];

            if (playerMap == null)
            {
                Log.Error("Invalid MapId: {0}", this.Player.MapId);
                this.Server.RemoveClient(this);
                return;
            }

            playerMap.AddObject(this.Player);
        }

        private void OnMoveByKeyboard(NetPacketBase packet)
        {
            packet.Position = 24;
            var posX = packet.Read<float>();
            var posY = packet.Read<float>();
            var posZ = packet.Read<float>();
            var forward = packet.Read<byte>();

            this.Player.DestinationPosition = new Core.Structures.Vector3(posX, posY, posZ);
            this.Player.SendMoverMoving();
        }
    }
}
