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
        /// Gets the type of the packets.
        /// </summary>
        /// <value>
        /// The type of the packets.
        /// </value>
        public static Dictionary<Type, PacketType> PacketsType { get; }

        /// <summary>
        /// Gets the packet attributes.
        /// </summary>
        /// <value>
        /// The packet attributes.
        /// </value>
        public static Dictionary<PacketType, PacketTypeAttribute> PacketAttributes { get; }

        /// <summary>
        /// Build a Packet object from a given head and data, and add serverclient info in it
        /// </summary>
        /// <param name="serverClient">the client calling this method</param>
        /// <param name="head">a well built head</param>
        /// <param name="data">raw bytes of data</param>
        /// <returns></returns>
        public static Packet Create(IServerConnection serverClient, Head head, byte[] data)
        {
            Hashtable dataDict =
                JsonConvert.DeserializeObject<Hashtable>(Encoding.UTF8.GetString(data, 0, head.Length));

            Packet pkt = Activator.CreateInstance(LegalPackets[head.Type], head.Type, dataDict, serverClient) as Packet;
            pkt.PacketHead = head;
            return pkt;
        }

        static PacketFactory()
        {
            LegalPackets = new Dictionary<PacketType, Type>();
            PacketsType = new Dictionary<Type, PacketType>();
            PacketAttributes = new Dictionary<PacketType, PacketTypeAttribute>();
            var types = typeof(Packet).GetTypeInfo().Assembly.GetTypes()
                .Where(t => t.GetTypeInfo().GetCustomAttributes<PacketTypeAttribute>().Any());
            foreach (var t in types)
            {
                var packetAttrs = t.GetTypeInfo().GetCustomAttributes<PacketTypeAttribute>();
                foreach (var attr in packetAttrs)
                {
                    var key = attr.PacketType;
                    LegalPackets[key] = t;
                    PacketAttributes[key] = attr;
                    PacketsType[t] = key;
                }
            }
        }
    }
}