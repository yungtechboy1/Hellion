using Hellion.Core.Structures;
using Hellion.World.Structures;

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
