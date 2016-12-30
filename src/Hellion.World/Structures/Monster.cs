using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Structures
{
    public class Monster : Mover
    {
        private Region region;

        /// <summary>
        /// Gets the monster's attributes.
        /// </summary>
        public Attributes Attributes { get; private set; }

        /// <summary>
        /// Creates a new monster instance.
        /// </summary>
        /// <param name="modelId">Monster model id</param>
        /// <param name="mapId">Monster parent map id</param>
        public Monster(int modelId, int mapId)
            : this(modelId, mapId, null)
        {
        }

        /// <summary>
        /// Create a new monster instance.
        /// </summary>
        /// <param name="modelId">Monster model id</param>
        /// <param name="mapId">Monster map id</param>
        /// <param name="parentRegion">Monster parent region</param>
        public Monster(int modelId, int mapId, Region parentRegion)
            : base(modelId)
        {
            this.MapId = mapId;
            this.region = parentRegion;
            this.Attributes = new Attributes();

            this.Position = this.region.GetRandomPosition();
            this.DestinationPosition = this.Position.Clone();
        }
    }
}
