using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Systems.Map
{
    public class MapManager
    {
        private static object syncLock = new object();

        private ICollection<Map> maps;

        public int Count
        {
            get { return this.maps.Count; }
        }

        public Map this[int id]
        {
            get
            {
                return this.maps.Where(x => x.Id == id).FirstOrDefault();
            }
        }

        public MapManager()
        {
            this.maps = new List<Map>();
        }

        public void AddMap(Map map)
        {
            lock (syncLock)
            {
                if (!this.maps.Contains(map))
                    this.maps.Add(map);
            }
        }

        public void RemoveMap(Map map)
        {
            lock (syncLock)
            {
                if (this.maps.Contains(map))
                    this.maps.Remove(map);
            }
        }

        public void RemoveMap(int id)
        {
            Map map = this.maps.Where(x => x.Id == id).FirstOrDefault();

            if (map != null)
                this.RemoveMap(map);
        }
    }
}
