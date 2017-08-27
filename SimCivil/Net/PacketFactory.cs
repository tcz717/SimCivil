using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using SimCivil.Net.Packets;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Collections;

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
        public static Packet Create(IServerConnection serverClient, Head head, byte[] data)
        {
            Hashtable dataDict = JsonConvert.DeserializeObject<Hashtable>(Encoding.UTF8.GetString(data, 0, head.length));

            Packet pkt = Activator.CreateInstance(LegalPackets[head.type], dataDict, serverClient) as Packet;
            pkt.Head = head;
            return pkt;
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