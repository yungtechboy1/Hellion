using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Structures
{
    public class Npc : Mover
    {
        public string Name { get; set; }

        public Npc()
            : base(-1)
        {
        }
    }
}
