using Hellion.Core;
using Hellion.Core.Data;
using Hellion.Core.Data.Resources;
using Hellion.World.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hellion.World.Systems
{
    public class Map
    {
        private Thread updateThread;

        private ICollection<Player> players;
        private object syncLockClient = new object();

        private ICollection<Npc> npcs;
        private object syncLockNpc = new object();

        /// <summary>
        /// Gets the map id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the map name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Creates a new Map instance with a name and id.
        /// </summary>
        /// <param name="id">Id of the map</param>
        /// <param name="mapName">Name of the map</param>
        public Map(int id, string mapName)
        {
            this.Id = id;
            this.Name = mapName;
            this.players = new HashSet<Player>();
            this.npcs = new HashSet<Npc>();
        }

        /// <summary>
        /// Load a map.
        /// </summary>
        public void Load()
        {
            string mapPath = Path.Combine(Global.DataPath, "maps", this.Name);
            string dyoMapPath = Path.Combine(mapPath, this.Name + ".dyo");
            
            byte[] dyoFileData = File.ReadAllBytes(dyoMapPath);
            var dyo = new DyoFile(dyoFileData);
            dyo.Read();

            foreach (NpcDyoElement dyoElement in dyo.Elements.Where(d => d is NpcDyoElement))
            {
                var npc = new Npc();
                npc.MapId = this.Id;
                npc.ModelId = dyoElement.Index;
                npc.Angle = dyoElement.Angle;
                npc.Position = dyoElement.Position.Clone();
                npc.DestinationPosition = dyoElement.Position.Clone();
                npc.Size = (short)(npc.Size * dyoElement.Scale.X);
                npc.Name = dyoElement.Name;
                this.npcs.Add(npc);
            }

            // Load .wld
            // Load .rgn
            // Load .lnd
        }

        /// <summary>
        /// Start the map update thread.
        /// </summary>
        public void StartThread()
        {
            if (this.updateThread == null)
            {
                this.updateThread = new Thread(this.Update);
                this.updateThread.Start();
            }
        }

        /// <summary>
        /// Adds a new <see cref="WorldObject"/> to this map.
        /// </summary>
        /// <param name="worldObject"></param>
        public void AddObject(WorldObject worldObject)
        {
            worldObject.IsSpawned = true;

            if (worldObject is Player)
            {
                lock (syncLockClient)
                    this.players.Add(worldObject as Player);
            }
        }

        /// <summary>
        /// Remove a <see cref="WorldObject"/> from this map and notify all the players spawned around him.
        /// </summary>
        /// <param name="worldObject"></param>
        public void RemoveObject(WorldObject worldObject)
        {
            if (worldObject is Player)
            {
                var player = worldObject as Player;

                lock (syncLockClient)
                {
                    this.players.Remove(player);
                    foreach (var otherPlayer in this.players)
                        otherPlayer.DespawnObject(player);
                }
            }

            worldObject.IsSpawned = false;
        }

        /// <summary>
        /// Update the map objects.
        /// </summary>
        public void Update()
        {
            while (true)
            {
                this.UpdatePlayerVisibility();

                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Update the player visibility with other players.
        /// </summary>
        private void UpdatePlayerVisibility()
        {
            lock (syncLockClient)
            {
                foreach (Player player in this.players)
                {
                    if (!player.IsSpawned)
                        continue;

                    player.Update();
                    foreach (WorldObject obj in this.players)
                    {
                        if (!obj.IsSpawned || player.GetHashCode() == obj.GetHashCode())
                            continue;

                        if (player.CanSee(obj))
                        {
                            if (!player.SpawnedObjects.Contains(obj))
                                player.SpawnObject(obj);
                        }
                        else
                            player.DespawnObject(obj);
                    }

                    lock (syncLockNpc)
                    {
                        foreach (var npc in this.npcs)
                        {
                            if (player.CanSee(npc))
                            {
                                if (!player.SpawnedObjects.Contains(npc))
                                    player.SpawnObject(npc);
                            }
                            else
                                player.DespawnObject(npc);
                        }
                    }
                }
            }
        }
    }
}
