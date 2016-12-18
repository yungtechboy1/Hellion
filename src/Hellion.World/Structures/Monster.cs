using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Structures
{
    public class Monster : Mover
    {
        /// <summary>
        /// Gets the monster's attributes.
        /// </summary>
        public Attributes Attributes { get; private set; }

        /// <summary>
        /// Create a new monster instance.
        /// </summary>
        public Monster()
            : base(-1)
        {
            this.Attributes = new Attributes();
        }
    }
}
