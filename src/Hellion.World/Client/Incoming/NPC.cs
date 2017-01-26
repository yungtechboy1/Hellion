using Hellion.Core.Data.Headers;
using Hellion.Core.IO;
using Hellion.Core.Network;
using Hellion.World.Structures;
using Hellion.World.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Client
{
    public partial class WorldClient
    {
        [FFIncomingPacket(WorldHeaders.Incoming.NPCInteraction)]
        public void OnNPCInteraction(FFPacket packet)
        {
            var objectId = packet.Read<int>();
            var dialogKey = packet.Read<string>();
            var global1 = packet.Read<int>();
            var global2 = packet.Read<int>();
            var global3 = packet.Read<int>();
            var global4 = packet.Read<int>();

            Log.Debug("Obj: {0} ; Name: {1} ; g1: {2} ; g2: {3} ; srcId: {4} ; destId: {5}", 
                objectId, 
                dialogKey, 
                global1, 
                global2, 
                global3, 
                global4);

            var npc = this.Player.GetSpawnedObjectById<Npc>(objectId);

            if (npc != null)
            {
                if (dialogKey == "BYE")
                {
                    var byeLink = npc.Dialog.Links.Where(x => x.Id == dialogKey).FirstOrDefault();

                    npc.SendNormalChatTo(byeLink?.Text, this.Player);
                    npc.SendCloseDialogTo(this.Player);
                }
                else
                    npc.SendDialogTo(this.Player, dialogKey);
            }
        }
    }
}
