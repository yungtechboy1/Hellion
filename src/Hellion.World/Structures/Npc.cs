using Hellion.Core.Data.Resources;
using Hellion.Core.Structures.Dialogs;
using System.Collections.Generic;

namespace Hellion.World.Structures
{
    /// <summary>
    /// FlyFF NPC structure.
    /// </summary>
    public class Npc : Mover
    {
        /// <summary>
        /// Gets the NPC name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the NPC data.
        /// </summary>
        public NPCData Data { get; private set; }

        /// <summary>
        /// Gets or sets the npc dialog data.
        /// </summary>
        public DialogData Dialog { get; private set; }

        /// <summary>
        /// Gets the NPC shop items.
        /// </summary>
        /// <remarks>
        /// We know that a shop is composed of a maximum of 4 tabs and can have 100 items max in each tab.
        /// So we this property is an array of a collection with a variable size.
        /// </remarks>
        public ICollection<Item>[] Shop { get; set; }

        /// <summary>
        /// Creates a new NPC instance.
        /// </summary>
        public Npc()
            : base(-1)
        {
        }

        /// <summary>
        /// Update the NPC.
        /// </summary>
        public override void Update()
        {
            base.Update();
        }

        /// <summary>
        /// Creates a new NPC from a dyo element.
        /// </summary>
        /// <param name="dyoElement">Map Dyo element</param>
        /// <param name="mapId">Npc map Id</param>
        /// <returns></returns>
        public static Npc CreateFromDyo(NpcDyoElement dyoElement, int mapId)
        {
            var npc = new Npc();

            npc.MapId = mapId;
            npc.ModelId = dyoElement.Index;
            npc.Angle = dyoElement.Angle;
            npc.Position = dyoElement.Position.Clone();
            npc.DestinationPosition = dyoElement.Position.Clone();
            npc.Size = (short)(npc.Size * dyoElement.Scale.X);
            npc.Name = dyoElement.Name;

            if (WorldServer.NPCData.ContainsKey(npc.Name))
                npc.Data = WorldServer.NPCData[npc.Name];

            if (WorldServer.DialogsData.ContainsKey(npc.Name))
                npc.Dialog = WorldServer.DialogsData[npc.Name];

            return npc;
        }
    }
}
