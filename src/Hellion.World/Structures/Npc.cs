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
    }
}
