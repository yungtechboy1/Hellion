﻿using Ether.Network.Packets;
using Hellion.Core.Data.Headers;
using Hellion.Core.IO;
using Hellion.Core.Structures;
using Hellion.Database;
using Hellion.Database.Structures;
using Hellion.World.Client;
using Hellion.World.Managers;
using Hellion.World.Structures;
using System;

namespace Hellion.World.Systems
{
    /// <summary>
    /// Represents a real player in the world.
    /// </summary>
    public sealed partial class Player : Mover
    {
        /// <summary>
        /// Gets the parent client instance.
        /// </summary>
        public WorldClient Client { get; private set; }

        /// <summary>
        /// Gets the player Id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the player's account Id.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Gets the player's gender.
        /// </summary>
        public byte Gender { get; set; }

        /// <summary>
        /// Gets the player's amount of experience.
        /// </summary>
        public long Experience { get; set; }

        /// <summary>
        /// Gets the player's class Id.
        /// </summary>
        public int ClassId { get; set; }

        /// <summary>
        /// Gets the player's amount of gold.
        /// </summary>
        public int Gold { get; set; }

        /// <summary>
        /// Gets the player's slot.
        /// </summary>
        public int Slot { get; set; }

        /// <summary>
        /// Gets the player's authority.
        /// </summary>
        public int Authority { get; set; }

        /// <summary>
        /// Gets the player's skin set id.
        /// </summary>
        public int SkinSetId { get; set; }

        /// <summary>
        /// Gets the player's hair mesh id.
        /// </summary>
        public int HairId { get; set; }

        /// <summary>
        /// Gets the player's hair color.
        /// </summary>
        public uint HairColor { get; set; }

        /// <summary>
        /// Gets the player's face Id.
        /// </summary>
        public int FaceId { get; set; }

        /// <summary>
        /// Gets the player's bank code.
        /// </summary>
        public int BankCode { get; set; }

        /// <summary>
        /// Gets the player's chat module.
        /// </summary>
        public Chat Chat { get; private set; }

        /// <summary>
        /// Gets the player's inventory.
        /// </summary>
        public Inventory Inventory { get; private set; }

        // Add:
        // Quests
        // Guild
        // Friends
        // Skills
        // Buffs
        // etc...

        public override float FlightSpeed
        {
            get
            {
                var flyItem = this.Inventory.GetItemBySlot(55);

                if (flyItem == null)
                    return 0f;

                return flyItem.Data.FlightSpeed * 0.75f;
            }
        }

        /// <summary>
        /// Creates a new Player based on a <see cref="DbCharacter"/> stored in database.
        /// </summary>
        /// <param name="parentClient">Parent client instance</param>
        /// <param name="dbCharacter">Character stored in database</param>
        public Player(WorldClient parentClient, DbCharacter dbCharacter)
            : base(dbCharacter?.Gender == 0 ? 11 : 12)
        {
            this.Client = parentClient;
            this.Chat = new Chat(this);
            this.Inventory = new Inventory(this, dbCharacter.Items);

            this.Id = dbCharacter.Id;
            this.AccountId = dbCharacter.AccountId;
            this.Name = dbCharacter.Name;
            this.Gender = dbCharacter.Gender;
            this.ClassId = dbCharacter.ClassId;
            this.Gold = dbCharacter.Gold;
            this.Slot = dbCharacter.Slot;
            this.Level = dbCharacter.Level;
            this.Authority = this.Client.CurrentUser.Authority;
            this.Attributes[DefineAttributes.STR] = dbCharacter.Strength;
            this.Attributes[DefineAttributes.STA] = dbCharacter.Stamina;
            this.Attributes[DefineAttributes.DEX] = dbCharacter.Dexterity;
            this.Attributes[DefineAttributes.INT] = dbCharacter.Intelligence;
            this.Attributes[DefineAttributes.HP] = dbCharacter.Hp;
            this.Attributes[DefineAttributes.MP] = dbCharacter.Mp;
            this.Attributes[DefineAttributes.FP] = dbCharacter.Fp;
            this.Experience = dbCharacter.Experience;
            this.SkinSetId = dbCharacter.SkinSetId;
            this.HairId = dbCharacter.HairId;
            this.HairColor = dbCharacter.HairColor;
            this.FaceId = dbCharacter.FaceId;
            this.BankCode = dbCharacter.BankCode;
            this.MapId = dbCharacter.MapId;
            this.Position = new Vector3(dbCharacter.PosX, dbCharacter.PosY, dbCharacter.PosZ);
            this.Angle = dbCharacter.Angle;
            this.DestinationPosition = this.Position.Clone();
            this.IsFlying = this.Inventory.HasFlyingObjectEquiped();

            // Initialize quests, guild, friends, skills etc...
        }

        /// <summary>
        /// Disconnect the current player from the world.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                this.Save();

                var map = WorldServer.MapManager[this.MapId];

                if (map != null)
                    map.RemoveObject(this);

                // release targets
                foreach (Mover mover in this.SpawnedObjects)
                {
                    if (mover.TargetMover == this)
                        mover.RemoveTarget();
                }

                this.SpawnedObjects.Clear();
            }
            catch (Exception e)
            {
                Log.Error("An error occured while disconnecting the player. {0}", e.Message);
                Log.Debug("StackTrace: {0}", e.StackTrace);
            }
        }

        /// <summary>
        /// Send a packet to the player.
        /// </summary>
        /// <param name="packet"></param>
        public void Send(NetPacketBase packet)
        {
            try
            {
                if (this.Client.Socket != null && this.Client.Socket.Connected)
                    this.Client.Send(packet);
            }
            catch { }
        }

        /// <summary>
        /// Send packets to every visible players around this player.
        /// </summary>
        /// <param name="packet"></param>
        public override void SendToVisible(NetPacketBase packet)
        {
            this.Send(packet);
            base.SendToVisible(packet);
        }

        /// <summary>
        /// Save the player's informations into the database.
        /// </summary>
        public void Save()
        {
            var dbCharacter = DatabaseService.Characters.Get(c => c.Id == this.Id);

            if (dbCharacter == null)
                Log.Error("Save: Cannot save character with id: {0}", this.Id);
            else
            {
                dbCharacter.BankCode = this.BankCode;
                dbCharacter.Experience = this.Experience;
                dbCharacter.FaceId = this.FaceId;
                dbCharacter.Fp = this.Attributes[DefineAttributes.FP];
                dbCharacter.Gender = this.Gender;
                dbCharacter.Gold = this.Gold;
                dbCharacter.HairColor = this.HairColor;
                dbCharacter.HairId = this.HairId;
                dbCharacter.Hp = this.Attributes[DefineAttributes.HP];
                dbCharacter.Intelligence = this.Attributes[DefineAttributes.INT];
                dbCharacter.Level = this.Level;
                dbCharacter.MapId = this.MapId;
                dbCharacter.Mp = this.Attributes[DefineAttributes.MP];
                dbCharacter.Name = this.Name;
                dbCharacter.PosX = this.Position.X;
                dbCharacter.PosY = this.Position.Y;
                dbCharacter.PosZ = this.Position.Z;
                dbCharacter.Angle = this.Angle;
                dbCharacter.SkinSetId = this.SkinSetId;
                dbCharacter.Slot = this.Slot;
                dbCharacter.Stamina = this.Attributes[DefineAttributes.STA];
                dbCharacter.Strength = this.Attributes[DefineAttributes.STR];

                this.Inventory.Save();
                // TODO: save skills
                // TODO: save quest states

                DatabaseService.Characters.Update(dbCharacter, true);
            }
        }

        /// <summary>
        /// Spawn an object around this player.
        /// </summary>
        /// <param name="worldObject"></param>
        public void SpawnObject(WorldObject worldObject)
        {
            this.SpawnedObjects.Add(worldObject);

            if (worldObject is Player)
                this.SendPlayerSpawn(worldObject as Player);
            if (worldObject is Npc)
                (worldObject as Npc).SendSpawnTo(this);
            if (worldObject is Monster)
                this.SendMonsterSpawn(worldObject as Monster);

            if (worldObject is Mover)
            {
                var worldMover = worldObject as Mover;

                if (worldMover.Position != worldMover.DestinationPosition)
                    worldMover.SendMoverMoving();
            }
        }

        /// <summary>
        /// Despawn an object around this player.
        /// </summary>
        /// <param name="obj"></param>
        public override void DespawnObject(WorldObject obj)
        {
            this.SendDespawnObject(obj);

            base.DespawnObject(obj);
        }

        public override void Fight(Mover defender)
        {
            var rightWeapon = this.Inventory.GetItemBySlot(Inventory.RightWeaponSlot);

            if (rightWeapon == null)
                rightWeapon = Inventory.Hand;

            int damages = BattleManager.CalculateDamages(this, defender);

            // Set monster target
            if (defender is Monster && defender.TargetMover == null)
            {
                defender.Target(this);
                defender.IsFighting = true;
                defender.IsFollowing = true;
            }

            Log.Debug("{0} inflicted {1} damages to {2}", this.Name, damages, defender.Name);

        }
    }
}
