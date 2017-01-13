using Hellion.Core;
using Hellion.Core.Structures;

namespace Hellion.World.Structures
{
    public abstract class Region
    {
        public Vector3 Middle { get; protected set; }

        public Vector3 NorthEast { get; protected set; }

        public Vector3 SouthWest { get; protected set; }

        public Region(Vector3 middle, Vector3 northEast, Vector3 southWest)
        {
            this.Middle = middle;
            this.NorthEast = northEast;
            this.SouthWest = southWest;
        }

        public Vector3 GetRandomPosition()
        {
            var position = new Vector3();
            
            position.X = CRandom.FloatRandom(this.SouthWest.X, this.NorthEast.X);
            position.Y = this.Middle.Y;
            position.Z = CRandom.FloatRandom(this.SouthWest.Z, this.NorthEast.Z);

            return position;
        }

        public abstract void Update();
    }
}
