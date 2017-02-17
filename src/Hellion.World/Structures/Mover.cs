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
    public class Mover : WorldObject
    {
        private long nextMove;
        private long lastMoveTime;

        public bool IsDead { get; set; }

        public bool IsFlying { get; set; }

        public bool IsFighting { get; set; }

        public bool IsFollowing { get; set; }

        public bool IsReseting { get; set; }

        public bool IsMovingWithKeyboard { get; set; }

        public float Speed
        {
            get
            {
                float speed = WorldServer.MonstersData[this.ModelId].Speed;

                return speed * 10f;
            }
        }

        public virtual float FlightSpeed { get; }

        public ObjectState MovingFlags { get; set; }

        public StateFlags MotionFlags { get; set; }

        public int ActionFlags { get; set; }

        public Mover TargetMover { get; private set; }

        public float FollowDistance { get; set; }

        public virtual int Level { get; protected set; }

        public virtual string Name { get; set; }

        public Vector3 DestinationPosition { get; set; }

        public override WorldObjectType Type
        {
            get { return WorldObjectType.Mover; }
        }

        public Mover(int modelId)
            : base(modelId)
        {
            this.nextMove = Time.GetCurrentTick() + 10;
            this.Level = 1;
            this.DestinationPosition = new Vector3();
            this.TargetMover = null;
            this.FollowDistance = 1f;
            this.MovingFlags = ObjectState.OBJSTA_STAND;
        }

        public void Target(Mover mover)
        {
            this.TargetMover = mover;
        }

        public void RemoveTarget()
        {
            this.TargetMover = null;
        }

        public virtual void Update()
        {
            this.ProcessMoves();
        }



        private void ProcessMoves()
        {
            if (this.DestinationPosition.IsZero())
                return;

            if (this.nextMove > Time.GetCurrentTick())
                return;

            this.nextMove = Time.GetCurrentTick() + 10;

            if (this.IsFollowing)
                this.Follow();

            if (this.IsFlying)
                this.Fly();
            else
                this.Walk();
        }

        private void Fly()
        {
            if (this.FlightSpeed > 0 && this.MovingFlags.HasFlag(ObjectState.OBJSTA_FMOVE))
            {
                Vector3 distance = this.DestinationPosition - this.Position;
                Vector3 moveVector = this.Position.Clone();
                float angle = Vector3.AngleBetween(this.Position, this.DestinationPosition);
                float angleFly = this.AngleFly;
                float angleTheta = MathHelper.ToRadians(angle);
                float angleFlyTheta = MathHelper.ToRadians(angleFly);
                float turnAngle = 0f;
                float accelPower = 0f;


                switch (this.MovingFlags & ObjectState.OBJSTA_MOVE_ALL)
                {
                    case ObjectState.OBJSTA_STAND:
                        accelPower = 0f;
                        break;
                    case ObjectState.OBJSTA_FMOVE:
                        accelPower = this.FlightSpeed;
                        break;
                }

                switch (this.MovingFlags & ObjectState.OBJSTA_TURN_ALL)
                {
                    case ObjectState.OBJSTA_RTURN:
                        turnAngle = this.TurnAngle;
                        if (this.MotionFlags.HasFlag(StateFlags.OBJSTAF_ACC))
                            turnAngle *= 2.5f;
                        angle += turnAngle;
                        if (angle < 0.0f)
                            angle += 360.0f;
                        break;
                    case ObjectState.OBJSTA_LTURN:
                        turnAngle = this.TurnAngle;
                        if (this.MotionFlags.HasFlag(StateFlags.OBJSTAF_ACC))
                            turnAngle *= 2.5f;
                        angle += turnAngle;
                        if (angle > 360.0f)
                            angle -= 360.0f;
                        break;
                }

                switch (this.MovingFlags & ObjectState.OBJSTA_LOOK_ALL)
                {
                    case ObjectState.OBJSTA_LOOKUP:
                        if (angleFly > 45f)
                            angleFly -= 1f;
                        break;
                    case ObjectState.OBJSTA_LOOKDOWN:
                        if (angleFly < 45f)
                            angleFly += 1f;
                        break;
                }

                if (this.MotionFlags.HasFlag(StateFlags.OBJSTAF_TURBO))
                    accelPower *= 1.5f;

                float d = (float)Math.Cos(angleFlyTheta) * accelPower;

                var deltaVector = new Vector3();
                var accelVector = new Vector3()
                {
                    X = (float)Math.Sin(angleTheta) * d,
                    Y = (float)-Math.Sin(angleFlyTheta) * accelPower,
                    Z = (float)-Math.Cos(angleTheta) * d
                };
                var accelVectorNorm = accelVector.Normalize();
                var deltaVectorNorm = deltaVector.Normalize();
                float deltaVectorLength = deltaVector.GetLengthSq();
                float maxSpeed = 0.3f;

                if (this.MotionFlags.HasFlag(StateFlags.OBJSTAF_TURBO))
                    maxSpeed *= 1.1f;

                if (deltaVectorLength < (maxSpeed * maxSpeed))
                    deltaVector += accelVector;

                deltaVector *= (1.0f - 0.011f);

                if (this is Player)
                    moveVector += deltaVector;
                else
                {
                    // other ?
                }

                if (moveVector.Y > Map.MaxHeight)
                    moveVector.Y = Map.MaxHeight;

                this.Position = moveVector.Clone();
                this.AngleFly = angleFly;
                this.Angle = angle;
            }
        }

        private void Walk()
        {
            if (this.MovingFlags.HasFlag(ObjectState.OBJSTA_STAND))
                return;

            this.lastMoveTime = Time.GetTick();

            float angle = this.IsMovingWithKeyboard ?
               this.Angle : Vector3.AngleBetween(this.Position, this.DestinationPosition);

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
                this.DestinationPosition = this.Position.Clone();
                this.Angle = angle;
                v.Reset();

                if (!this.IsFighting)
                {
                    this.MovingFlags &= ~ObjectState.OBJSTA_FMOVE;
                    this.MovingFlags |= ObjectState.OBJSTA_STAND;
                    this.SendMoverAction((int)OBJMSG.OBJMSG_STAND);
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
        }

        private void Follow()
        {
            if (this.TargetMover != null)
            {
                this.DestinationPosition = this.TargetMover.Position;
                this.MovingFlags = this.MovingFlags & ~ObjectState.OBJSTA_STAND;
                this.MovingFlags = this.MovingFlags | ObjectState.OBJSTA_FMOVE;
            }
            else
            {
                this.IsFollowing = false;
                this.IsFighting = false;
                this.RemoveTarget();
                this.DestinationPosition.Reset();
                this.MovingFlags = ObjectState.OBJSTA_STAND;
                this.SendMoverAction((int)ObjectState.OBJSTA_STAND);
            }
        }

        // TODO: clean this mess up! :p

        private void move(float x, float z)
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

        public virtual void Fight(Mover defender) { }

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

        internal void SendFollowTarget(float distance)
        {
            if (this.TargetMover == null)
                return;

            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.MOVERSETDESTOBJ);
                packet.Write(this.TargetMover.ObjectId);
                packet.Write(distance);

                base.SendToVisible(packet);
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

        internal void SendMeleeAttack(int motion, int targetId)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.MELEE_ATTACK);
                packet.Write(motion);
                packet.Write(targetId);
                packet.Write(0);
                packet.Write(0x10000);

                this.SendToVisible(packet);
            }
        }

        internal void SendSpeed(float speed)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.SET_SPEED_FACTOR);
                packet.Write(speed);

                this.SendToVisible(packet);
            }
        }
    }
}
