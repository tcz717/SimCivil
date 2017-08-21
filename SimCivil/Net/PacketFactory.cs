using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using SimCivil.Net.Packets;
using System.Text;
using System.Reflection;
using System.Linq;

namespace SimCivil.Net
{
    /// <summary>
    /// Static class used to create Packet from binary data.
    /// </summary>
    public static class PacketFactory
    {
        /// <summary>
        /// Packet types allowed to construct in Factory.
        /// </summary>
        public static Dictionary<PacketType, Type> LegalPackets { get; }

        /// <summary>
        /// Build a Packet object from a given head and data, and add serverclient info in it
        /// </summary>
        /// <param name="serverClient">the client calling this method</param>
        /// <param name="head">a well built head</param>
        /// <param name="data">raw bytes of data</param>
        /// <returns></returns>
        public static Packet Create(ServerClient serverClient, Head head, byte[] data)
        {
            Dictionary<string, object> dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(Encoding.UTF8.GetString(data));

            return Activator.CreateInstance(LegalPackets[head.type], dataDict, head, serverClient) as Packet;
        }        
        
        static PacketFactory()
        {
            var dict = new Dictionary<PacketType, Type>();
            var types = typeof(Packet).GetTypeInfo().Assembly.GetTypes()
                .Where(t => t.GetTypeInfo().GetCustomAttribute<PacketTypeAttribute>() != null);
            foreach (var t in types)
            {
                dict[t.GetTypeInfo().GetCustomAttribute<PacketTypeAttribute>().PacketType] = t;
            }

            LegalPackets = dict;    
        }
    }
}