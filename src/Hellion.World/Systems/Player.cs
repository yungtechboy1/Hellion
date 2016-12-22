using Hellion.Core.Database;
using Hellion.Core.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hellion.Core.Data;
using Hellion.Core.IO;
using Ether.Network.Packets;
using Hellion.Core.Network;
using Hellion.World.Structures;
using Hellion.World.Modules;
using Hellion.World.Client;
using Hellion.Core.Data.Headers;

namespace Hellion.World.Systems
{
    /// <summary>
    /// Represents a real player in the world.
    /// </summary>
    public partial class Player : Mover
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
        /// Gets the Player name.
        /// </summary>
        public string Name { get; set; }

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
        /// Gets the player's attributes.
        /// </summary>
        public Attributes Attributes { get; private set; }

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

        /// <summary>
        /// Creates a new Player based on a <see cref="DbCharacter"/> stored in database.
        /// </summary>
        /// <param name="parentClient">Parent client instance</param>
        /// <param name="dbCharacter">Character stored in database</param>
        public Player(WorldClient parentClient, DbCharacter dbCharacter)
            : base(dbCharacter?.Gender == 0 ? 11 : 12)
        {
            this.Client = parentClient;
            this.Attributes = new Attributes();
            this.Inventory = new Inventory(this, dbCharacter.Items);

            this.Id = dbCharacter.Id;
            this.AccountId = dbCharacter.AccountId;
            this.Name = dbCharacter.Name;
            this.Gender = dbCharacter.Gender;
            this.ClassId = dbCharacter.ClassId;
            this.Gold = dbCharacter.Gold;
            this.Slot = dbCharacter.Slot;
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
            this.DestinationPosition = this.Position.Clone();

            // Initialize inventory, quests, guild, friends, skills etc...
        }

        /// <summary>
        /// Disconnect the current player from the world.
        /// </summary>
        public void Disconnect()
        {
            this.Save();

            var map = WorldServer.MapManager[this.MapId];

            if (map != null)
                map.RemoveObject(this);

            this.SpawnedObjects.Clear();
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
                dbCharacter.SkinSetId = this.SkinSetId;
                dbCharacter.Slot = this.Slot;
                dbCharacter.Stamina = this.Attributes[DefineAttributes.STA];
                dbCharacter.Strength = this.Attributes[DefineAttributes.STR];
                
                // TODO: save inventory
                // TODO: save skills
                // TODO: save quest states

                DatabaseService.Characters.Update(dbCharacter);
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
                this.SendNpcSpawn(worldObject as Npc);

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
    }
}
