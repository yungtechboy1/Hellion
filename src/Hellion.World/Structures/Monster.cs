using Hellion.Core;
using Hellion.Core.Data.Headers;
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
        /// Gets the monster's data.
        /// </summary>
        public MonsterData Data
        {
            get { return WorldServer.MonstersData.ContainsKey(this.ModelId) ? WorldServer.MonstersData[this.ModelId] : new MonsterData(); }
        }

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

            this.Attributes[DefineAttributes.HP] = this.Data.AddHp;
            this.Attributes[DefineAttributes.MP] = this.Data.AddMp;
            this.Attributes[DefineAttributes.STR] = this.Data.Str;
            this.Attributes[DefineAttributes.STA] = this.Data.Sta;
            this.Attributes[DefineAttributes.INT] = this.Data.Int;
            this.Attributes[DefineAttributes.DEX] = this.Data.Dex;
            this.Attributes[DefineAttributes.SPEED] = 50;
            this.Size = (short)(this.Data.Size + 100);
            this.Speed = this.Data.Speed;

            this.Position = this.region.GetRandomPosition();
            this.DestinationPosition = this.Position.Clone();
            this.Angle = CRandom.Random(0, 360);
        }
    }
}
