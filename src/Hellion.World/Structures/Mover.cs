using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hellion.Core.Data;
using Hellion.Core.Structures;
using Hellion.Core.IO;

namespace Hellion.World.Structures
{
    public abstract class Mover : WorldObject
    {
        private long lastMoveTime;

        public bool IsDead { get; set; }

        public bool IsFlying { get; set; }

        public bool IsFighting { get; set; }

        public bool IsFollowing { get; set; }

        public bool IsReseting { get; set; }

        public float Speed { get; set; }
        
        public int Level { get; }

        public Vector3 DestinationPosition { get; set; }

        public override WorldObjectType Type
        {
            get { return WorldObjectType.Mover; }
        }

        public Mover(int modelId)
            : base(modelId)
        {
            this.Speed = 0.1f;
            this.Level = 1;
            this.DestinationPosition = new Vector3();
        }

        public virtual void Update()
        {
            //this.Move();
        }

        private void Move()
        {
            float distanceX = this.DestinationPosition.X - this.Position.X;
            float distanceZ = this.DestinationPosition.Z - this.Position.Z;

            float fDistAll = (float)Math.Sqrt(distanceX * distanceX + distanceZ * distanceZ);
            double moveTime1 = Time.GetTick() - this.lastMoveTime;
            this.lastMoveTime = Time.GetTick();
            double moveTime2 = fDistAll / (100 * 0.0006 * this.Speed);

            if (moveTime2 <= moveTime1 || fDistAll == 0 || this.Position.IsInCircle(this.DestinationPosition, 0.2f))
            {
                this.DestinationPosition = this.Position;
            }
            else
            {
                float moveX = distanceX * ((float)moveTime1 / (float)moveTime2);
                float moveZ = distanceZ * ((float)moveTime1 / (float)moveTime2);
                this.Position.X += moveX;
                this.Position.Z += moveZ;
            }
        }
    }
}
