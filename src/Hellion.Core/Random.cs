using Hellion.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.Core
{
    public static class CRandom
    {
        /// <summary>
        /// Do a random between integers
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Int32 Random(Int32 min, Int32 max)
        {
            return (Int32)(min + Math.Floor(MersenneTwister.NextDouble() * (max - min + 1)));
        }

        /// <summary>
        /// Do a random between floats
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns></returns>
        public static Single FloatRandom(Single f1, Single f2)
        {
            return (f2 - f1) * (Single)MersenneTwister.NextDouble() + f1;
        }

        /// <summary>
        /// Do a random between long values
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Int64 LongRandom(Int64 min, Int64 max)
        {
            return (Int64)(min + Math.Floor(MersenneTwister.NextDouble() * (max - min + 1)));
        }
    }
}
