using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hellion.Core.Network
{
    public static class FFPacketHandler
    {
        public static Dictionary<object, MethodInfo> RegisteredPackets;

        static FFPacketHandler()
        {
            RegisteredPackets = new Dictionary<object, MethodInfo>();
        }

        public static void Initialize<T>()
        {
            var typeInfo = typeof(T).GetTypeInfo();
            MethodInfo[] allMethods = typeInfo.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            var methodsWithAttribute = from x in allMethods
                                       where x.GetCustomAttribute<FFIncomingPacketAttribute>() != null
                                       select x;

            foreach (var method in methodsWithAttribute)
            {
                var attribute = method.GetCustomAttribute<FFIncomingPacketAttribute>();

                if (RegisteredPackets.ContainsKey(attribute.Header))
                    throw new Exception("The key " + attribute.Header.ToString() + " already exists");

                RegisteredPackets.Add(attribute.Header, method);
            }
        }

        public static bool Invoke<T>(T instance, object header, object parameter)
        {
            if (!RegisteredPackets.ContainsKey(header))
                return false;

            RegisteredPackets[header].Invoke(instance, new object[] { parameter });

            return true;
        }
    }
}
