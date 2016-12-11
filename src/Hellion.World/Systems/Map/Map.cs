using Hellion.Core;
using Hellion.Core.Data;
using Hellion.World.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hellion.World.Systems.Map
{
    public class Map
    {
        private Thread thread;

        private ICollection<Player> players;
        private object syncLockClient = new object();

        /// <summary>
        /// Gets the map id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the map name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the spawned objects of the map.
        /// </summary>
        //public ICollection<WorldObject> Objects { get; private set; }

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
            //this.Objects = new HashSet<WorldObject>();
        }

        /// <summary>
        /// Load a map.
        /// </summary>
        public void Load()
        {
            string mapPath = Path.Combine(Global.DataPath, "maps", this.Name);
            
            // Load .wld
            // Load .dyo
            // Load .rgn
            // Load .lnd
        }

        /// <summary>
        /// Start the map update thread.
        /// </summary>
        public void StartThread()
        {
            if (this.thread == null)
            {
                this.thread = new Thread(this.Update);
                this.thread.Start();
            }
        }

        public void AddObject(WorldObject worldObject)
        {
            worldObject.IsSpawned = true;

            if (worldObject is Player)
            {
                lock (syncLockClient)
                    this.players.Add(worldObject as Player);
            }
        }

        public void RemoveObject(WorldObject worldObject)
        {
            if (worldObject is Player)
            {
                lock (syncLockClient)
                    this.players.Remove(worldObject as Player);
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
                }
            }
        }
    }
}
