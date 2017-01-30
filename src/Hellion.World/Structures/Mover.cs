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
using Hellion.Core.Helpers;

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

        public bool IsMovingWithKeyboard { get; set; }

        public float Speed { get; set; }

        public ObjectState MovingFlags { get; set; }

        public StateFlags MotionFlags { get; set; }

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
            this.Speed = 0.5f;
            this.Level = 1;
            this.DestinationPosition = new Vector3();
        }

        public virtual void Update()
        {
            this.ProcessMoves();
        }

        private void ProcessMoves()
        {
            if (this.IsFlying)
                this.Fly();
            else
                this.Walk();
        }

        private void Fly()
        {
        }

        private void Walk()
        {
            this.lastMoveTime = Time.GetTick();
            
            // DEBUG
            //if (this.IsMovingWithKeyboard)
            //    f = this.Angle;
            //else
            //    GetDegree(ref f, DestinationPosition - Position);

            float distX = this.DestinationPosition.X - this.Position.X;
            float distZ = this.DestinationPosition.Z - this.Position.Z;
            float distAll = (float)Math.Sqrt(distX * distX + distZ * distZ);

            var distDelta = new Vector3();
            float angleTheta = (float)(this.Angle * (Math.PI / 180));
            float speed = this.Speed; // TODO: add speed bonus

            switch (this.MovingFlags & ObjectState.OBJSTA_MOVE_ALL)
            {
                case ObjectState.OBJSTA_FMOVE:
                    if (this.MotionFlags.HasFlag(StateFlags.OBJSTAF_WALK))
                    {
                        distDelta.X = (float)(Math.Sin(angleTheta) * (speed / 4f));
                        distDelta.Z = (float)(-Math.Cos(angleTheta) * (speed / 4f));
                    }
                    else
                    {
                        distDelta.X = (float)(Math.Sin(angleTheta) * (speed));
                        distDelta.Z = (float)(-Math.Cos(angleTheta) * (speed));
                    }
                    break;
                case ObjectState.OBJSTA_BMOVE:
                    distDelta.X = (float)(Math.Sin(angleTheta) * (speed / 5f));
                    distDelta.Z = (float)(-Math.Cos(angleTheta) * (speed / 5f));
                    break;
            }
            switch (this.MovingFlags & ObjectState.OBJSTA_TURN_ALL)
            {
                case ObjectState.OBJSTA_LTURN:
                    this.Angle += 4;
                    if (this.Angle > 360)
                        this.Angle -= 360;
                    break;
                case ObjectState.OBJSTA_RTURN:
                    this.Angle -= 4;
                    if (this.Angle < 0)
                        this.Angle += 360;
                    break;
            }

            float longprogression = (float)Math.Sqrt(distDelta.X * distDelta.X + distDelta.Z * distDelta.Z);
            
            if (distAll <= longprogression)
            {
                //Log.Debug("{0} Arrived", this.Name);
                this.DestinationPosition = this.Position.Clone();

                if (!this.IsFighting)
                {
                    MovingFlags &= ~ObjectState.OBJSTA_FMOVE; 
                    MovingFlags |= ObjectState.OBJSTA_STAND;
                    SendMoverAction((int)OBJMSG.OBJMSG_STAND);
                }
            }
            else
            {
                this.Position.X += distDelta.X;
                this.Position.Z += distDelta.Z;
                distAll -= longprogression;
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

        public void SendMoverAction(int motionId)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.MOTION);
                packet.Write(motionId);

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
