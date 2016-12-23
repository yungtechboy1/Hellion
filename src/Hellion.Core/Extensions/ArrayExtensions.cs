using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.Core.Extensions
{
    public static class ArrayExtensions
    {
        public static void Swap<T>(this T[] array, int source, int dest)
        {
            var temp = array[source];

            array[source] = array[dest];
            array[dest] = temp;
        }
    }
}
