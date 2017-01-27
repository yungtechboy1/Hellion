using Hellion.Core.Data.Headers;
using Hellion.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Systems
{
    public partial class Npc
    {
        public void SendSpawnTo(Player player)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.ObjectSpawn);

                packet.Write((byte)this.Type);
                packet.Write(this.ModelId);
                packet.Write((byte)this.Type);
                packet.Write(this.ModelId);
                packet.Write(this.Size);
                packet.Write(this.Position.X);
                packet.Write(this.Position.Y);
                packet.Write(this.Position.Z);
                packet.Write((short)(this.Angle * 10f));
                packet.Write(this.ObjectId);

                packet.Write<short>(1);
                packet.Write<byte>(0);
                packet.Write(1); // can be selected
                packet.Write(1);
                packet.Write(0);
                packet.Write<byte>(1);
                packet.Write(-1);
                packet.Write<byte>(0);//packet.Write((byte)this.Data.HairId);
                packet.Write(0);//packet.Write(this.Data.HairColor);
                packet.Write<byte>(0);//packet.Write((byte)this.Data.FaceId);
                packet.Write(this.Name);
                packet.Write<byte>(0); // item equiped count
                packet.Write<byte>(0);
                packet.Write<byte>(0);
                packet.Write<byte>(0);
                packet.Write(0);
                packet.Write<float>(1);
                packet.Write(0);

                player.Send(packet);
            }
        }

        public void SendDialogTo(Player player, string dialogKey)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.ObjectId, WorldHeaders.Outgoing.SNAPSHOTTYPE_RUNSCRIPTFUNC);
                packet.Write((short)DialogOptions.FUNCTYPE_REMOVEALLKEY);

                string dialogText = "";
                if (string.IsNullOrEmpty(dialogKey))
                    dialogText = this.Dialog.TextIntro;
                else
                {
                    var link = this.Dialog.Links.Where(x => x.Id == dialogKey).FirstOrDefault();

                    dialogText = link?.Text;
                }

                packet.StartNewMergedPacket(player.ObjectId, WorldHeaders.Outgoing.SNAPSHOTTYPE_RUNSCRIPTFUNC);
                packet.Write((short)DialogOptions.FUNCTYPE_SAY);
                packet.Write(dialogText);
                packet.Write(0); // quest id

                foreach (var link in this.Dialog.Links)
                {
                    packet.StartNewMergedPacket(player.ObjectId, WorldHeaders.Outgoing.SNAPSHOTTYPE_RUNSCRIPTFUNC);
                    packet.Write((short)DialogOptions.FUNCTYPE_ADDKEY);
                    packet.Write(link.Title);
                    packet.Write(link.Id);
                    packet.Write<long>(0);
                }

                player.Send(packet);
            }
        }

        public void SendCloseDialogTo(Player player)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.ObjectId, WorldHeaders.Outgoing.SNAPSHOTTYPE_RUNSCRIPTFUNC);
                packet.Write((short)DialogOptions.FUNCTYPE_EXIT);

                player.Send(packet);
            }
        }
    }
}
