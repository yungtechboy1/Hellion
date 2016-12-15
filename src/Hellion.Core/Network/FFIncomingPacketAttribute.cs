using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.Core.Network
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FFIncomingPacketAttribute : Attribute
    {
        public object Header { get; private set; }

        public FFIncomingPacketAttribute(object header)
        {
            this.Header = header;
        }
    }
}
