using Hellion.Core.Structures;
using Hellion.World.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Systems
{
    public class RespawnerRegion : Region
    {
        public RespawnerRegion(Vector3 middle, Vector3 northEast, Vector3 southWest, int respawnTime)
            : base(middle, northEast, southWest)
        {
        }

        public override void Update()
        {
        }
    }
}
