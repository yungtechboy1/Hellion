using System;

namespace Hellion.Core.Helpers
{
    public static class MathHelper
    {
        /// <summary>
        /// Converts a randian angle to degree.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToDegree(float value)
        {
            return (float)(value * 180f / Math.PI);
        }

        /// <summary>
        /// Converts a degree angle to radian.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToRadians(float value)
        {
            return (float)(Math.PI * value / 180f);
        }
    }
}
