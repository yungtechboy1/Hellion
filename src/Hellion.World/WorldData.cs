using Hellion.Core;
using Hellion.Core.Configuration;
using Hellion.Core.Data.Resources;
using Hellion.Core.IO;
using Hellion.Data;
using Hellion.World.Managers;
using Hellion.World.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hellion.World
{
    public partial class WorldServer
    {
        private Dictionary<string, int> defines = new Dictionary<string, int>();
        private static MapManager mapManager;

        /// <summary>
        /// Gets the Map manager.
        /// </summary>
        public static MapManager MapManager
        {
            get { return mapManager = mapManager ?? new MapManager(); }
        }


        /// <summary>
        /// Loads the world server data like resources, maps, quests, dialogs, etc...
        /// </summary>
        private void LoadData()
        {
            var startTime = DateTime.Now;
            Log.Info("Loading world data...");

            this.LoadDefines();
            this.LoadNpc();
            this.LoadMaps();
            this.Clear();

            Log.Done("World data loaded in {0}s", (DateTime.Now - startTime).TotalSeconds);
        }

        /// <summary>
        /// Clear all unused resources.
        /// </summary>
        private void Clear()
        {
            this.defines.Clear();
        }

        /// <summary>
        /// Load the flyff defines files.
        /// </summary>
        private void LoadDefines()
        {
            string[] defines = {
                                    "define.h",
                                    "defineAttribute.h",
                                    "defineEvent.h",
                                    "defineHonor.h",
                                    "defineItem.h",
                                    "defineItemkind.h",
                                    "defineJob.h",
                                    "defineNeuz.h",
                                    "defineObj.h",
                                    "defineSkill.h",
                                    "defineSound.h",
                                    "defineText.h",
                                    "defineWorld.h"
                                };

            foreach (var defineFile in defines)
            {
                var defineFileContent = new DefineFile(Path.Combine(Global.DataPath, "res", "data", defineFile));
                defineFileContent.Parse();

                foreach (var define in defineFileContent.Defines)
                {
                    if (!this.defines.ContainsKey(define.Key))
                        this.defines.Add(define.Key, define.Value);
                }
            }
        }

        /// <summary>
        /// Load all flyff npcs.
        /// </summary>
        private void LoadNpc()
        {
            Log.Info("Loading NPC data...");

            string[] files = {
                                  "data//res//dataSub1//character.inc",
                                  "data//res//dataSub1//character-etc.inc",
                                  "data//res//dataSub1//character-school.inc"
                              };

            foreach (var npcFile in files)
            {
                string path = Path.Combine(Global.DataPath, npcFile);
                var npcGroupFile = new ResourceGroup(npcFile);

                npcGroupFile.Parse();

                foreach (var npc in npcGroupFile.Groups)
                {
                    // Add npc here
                }
            }
        }

        /// <summary>
        /// Load the flyff maps specified in the configuration file.
        /// </summary>
        private void LoadMaps()
        {
            Log.Info("Loading maps...");

            IEnumerable<MapConfiguration> maps = this.WorldConfiguration.Maps;

            foreach (var map in maps)
            {
                Log.Loading("Loading map '{0}'...", map.Name);
                var newMap = new Map(map.Id, map.Name);
                newMap.Load();
                newMap.StartThread();

                MapManager.AddMap(newMap);
            }

            Log.Done("{0} maps loaded!", MapManager.Count);
        }
    }
}
