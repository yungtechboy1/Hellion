using System;

namespace Hellion.Core.Network
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FFIncomingPacketAttribute : Attribute
    {
        /// <summary>
        /// Gets the packet attribute header.
        /// </summary>
        public object Header { get; private set; }

        /// <summary>
        /// Creates a new FFIncomingPacketAttribute instance.
        /// </summary>
        /// <param name="header"></param>
        public FFIncomingPacketAttribute(object header)
        {
            this.Header = header;
        }
    }
}
