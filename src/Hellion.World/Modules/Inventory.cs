using Hellion.World.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Modules
{
    public class Inventory
    {
        public const int EquipOffset = 42;
        public const int MaxItems = 73;

        private Item[] items;


        public Inventory()
        {
            this.items = new Item[MaxItems];

            for (int i = 0; i < MaxItems; ++i)
                this.items[i] = new Item();
        }
    }
}
