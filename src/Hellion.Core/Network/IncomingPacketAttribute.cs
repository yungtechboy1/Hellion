using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.Core.Network
{
    [AttributeUsage(AttributeTargets.Method)]
    public class IncomingPacketAttribute : Attribute
    {
        public object Header { get; private set; }

        public IncomingPacketAttribute(object header)
        {
            this.Header = header;
        }
    }
}
