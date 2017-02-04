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

        private long nextMove = Time.GetCurrentTick() + 10;

        private void ProcessMoves()
        {
            if (this.DestinationPosition.IsZero())
                return;

            if (this.nextMove > Time.GetCurrentTick())
                return;

            this.nextMove = Time.GetCurrentTick() + 10;

            if (this.IsFlying)
                this.Fly();
            //else
                //this.Walk();
        }

        private void Fly()
        {
        }

        public static void xGetDegree(ref float pfAngXZ,/* ref float pfAngH,*/ Vector3 vDist)
        {
            Vector3 vDistXZ = vDist.Clone();
            //Helper.CopyVector(vDist, ref vDistXZ);
            vDistXZ.Y = 0;
            float fAngXZ = MathHelper.ToDegree((float)Math.Atan2(vDist.X, -vDist.Z));		// ¿ì¼± XZÆò¸éÀÇ °¢µµ¸¦ ¸ÕÀú ±¸ÇÔ
            float fLenXZ = vDistXZ.Length;					// yÁÂÇ¥¸¦ ¹«½ÃÇÑ XZÆò¸é¿¡¼­ÀÇ ±æÀÌ¸¦ ±¸ÇÔ.
            //float fAngH = ToDegree((float)Math.Atan2(fLenXZ, vDist.fPosY));     // XZÆò¸éÀÇ ±æÀÌ¿Í y³ôÀÌ°£ÀÇ °¢µµ¸¦ ±¸ÇÔ.

            //fAngH -= 90.0f;
            if (fAngXZ < 0)
                fAngXZ += 360;
            else if (fAngXZ >= 360)
                fAngXZ -= 360;

            pfAngXZ = fAngXZ;
            //pfAngH = fAngH;
        }

        private void Walk()
        {
            if (this.MovingFlags.HasFlag(ObjectState.OBJSTA_STAND))
                return;

            this.lastMoveTime = Time.GetTick();

            // DEBUG

            //if (this.IsMovingWithKeyboard)
            //    f = this.Angle;
            //else
            //    GetDegree(ref f, DestinationPosition - Position);
            float angle = 0;

            if (this.IsMovingWithKeyboard)
            {
                angle = this.Angle;
            }
            else
            {
                xGetDegree(ref angle, this.DestinationPosition - this.Position);

                if (angle == 180)
                {
                }
            }
            //float angle = this.IsMovingWithKeyboard ? this.Angle : Vector3.AngleBetween(this.Position, this.DestinationPosition);

            float distX = this.DestinationPosition.X - this.Position.X;
            float distZ = this.DestinationPosition.Z - this.Position.Z;
            float distAll = (float)Math.Sqrt(distX * distX + distZ * distZ);

            var distDelta = new Vector3();
            float angleTheta = (float)(angle * (Math.PI / 180));
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
                    angle += 4;
                    if (angle > 360)
                        angle -= 360;
                    break;
                case ObjectState.OBJSTA_RTURN:
                    angle -= 4;
                    if (angle < 0)
                        angle += 360;
                    break;
            }

            float longprogression = (float)Math.Sqrt(distDelta.X * distDelta.X + distDelta.Z * distDelta.Z);

            Vector3 v = new Vector3();
            if (distAll <= longprogression)
            {
                //Log.Debug("{0} Arrived", this.Name);
                this.DestinationPosition = this.Position.Clone();
                this.Angle = angle;
                v.Reset();

                if (!this.IsFighting)
                {
                    MovingFlags &= ~ObjectState.OBJSTA_FMOVE;
                    MovingFlags |= ObjectState.OBJSTA_STAND;
                    SendMoverAction((int)OBJMSG.OBJMSG_STAND);
                }
            }
            else
            {
                v.X += distDelta.X;
                v.Z += distDelta.Z;
                distAll -= longprogression;
            }

            this.move(v.X, v.Z);
            this.Angle = angle;

            if ((this is Player))
                Log.Debug("Mover Angle = {0}", this.Angle);
        }

        // TODO: clean this mess up! :p
         
        public void move(float x, float z)
        {
            if (this.IsMovingWithKeyboard)
            {
                if (this.MovingFlags.HasFlag(ObjectState.OBJSTA_BMOVE))
                {
                    this.Position.X -= (float)(Math.Sin(this.Angle * (Math.PI / 180)) * Math.Sqrt(x * x + z * z));
                    this.Position.Z += (float)(Math.Cos(this.Angle * (Math.PI / 180)) * Math.Sqrt(x * x + z * z));
                }
                else if (this.MovingFlags.HasFlag(ObjectState.OBJSTA_FMOVE))
                {
                    this.Position.X += (float)(Math.Sin(this.Angle * (Math.PI / 180)) * Math.Sqrt(x * x + z * z));
                    this.Position.Z -= (float)(Math.Cos(this.Angle * (Math.PI / 180)) * Math.Sqrt(x * x + z * z));
                }
            }
            else
            {
                this.Position.X += x;
                this.Position.Z += z;
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
