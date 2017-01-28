using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hellion.Core.Data;
using Hellion.Core.Structures;
using Hellion.Core.IO;
using Hellion.Core.Network;
using Hellion.Core.Data.Headers;
using Hellion.World.Systems;

namespace Hellion.World.Structures
{
    public partial class Mover : WorldObject
    {
        private long lastMoveTime;

        public bool IsDead { get; set; }

        public bool IsFlying { get; set; }

        public bool IsFighting { get; set; }

        public bool IsFollowing { get; set; }

        public bool IsReseting { get; set; }

        public float Speed { get; set; }

        public uint MovingFlags { get; set; }

        public int MotionFlags { get; set; }

        public int ActionFlags { get; set; }
        
        public int Level { get; }

        public virtual string Name { get; set; }

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
            this.Move();
        }

        private void Move()
        {
            float distance = (float)this.Position.GetDistance3D(this.DestinationPosition);
            float distanceX = this.DestinationPosition.X - this.Position.X;
            float distanceZ = this.DestinationPosition.Z - this.Position.Z;
            
            double moveTime1 = Time.GetTick() - this.lastMoveTime;
            this.lastMoveTime = Time.GetTick();
            double moveTime2 = distance / (100 * 0.0006 * this.Speed);

            if (moveTime2 <= moveTime1 || distance == 0 || this.Position.IsInCircle(this.DestinationPosition, 0.2f))
            {
                this.DestinationPosition = this.Position.Clone();
            }
            else
            {
                float moveX = distanceX * ((float)moveTime1 / (float)moveTime2);
                float moveZ = distanceZ * ((float)moveTime1 / (float)moveTime2);
                this.Position.X += moveX;
                this.Position.Z += moveZ;
            }
        }

        // TODO: Move this packets to an other file.

        internal void SendMoverMoving()
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.DESTPOS);
                packet.Write(this.DestinationPosition.X);
                packet.Write(this.DestinationPosition.Y);
                packet.Write(this.DestinationPosition.Z);
                packet.Write<byte>(1);

                this.SendToVisible(packet);
            }
        }

        internal void SendMoverPosition()
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.SETPOS);
                packet.Write(this.Position.X);
                packet.Write(this.Position.Y);
                packet.Write(this.Position.Z);

                this.SendToVisible(packet);
            }
        }

        private void SendNormalChat(string message, Player toPlayer = null)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.CHAT);
                packet.Write(message);

                if (toPlayer == null)
                    this.SendToVisible(packet);
                else
                    toPlayer.Send(packet);
            }
        }

        internal void SendNormalChat(string message)
        {
            this.SendNormalChat(message, null);
        }

        internal void SendNormalChatTo(string message, Player player)
        {
            this.SendNormalChat(message, player);
        }
    }
}
