using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using SimCivil.Net.Packets;

namespace SimCivil.Net
{
    public class PacketFactory
    {
        /// <summary>
        /// Build a Packet object from a given head and data, and add serverclient info in it
        /// </summary>
        /// <param name="serverClient">the client calling this method</param>
        /// <param name="head">a well built head</param>
        /// <param name="data">raw bytes of data</param>
        /// <returns></returns>
        public static Packet Create(ServerClient serverClient, Head head, byte[] data)
        {
            JsonSerializer ser = new JsonSerializer();
            Packet packet;
            MemoryStream stream = new MemoryStream();
            Dictionary<string, object> dataDict;
            using (StreamReader sr = new StreamReader(stream))
            {
                stream.Write(data, 0, head.length);
                stream.Position = 0;
                dataDict = (Dictionary<string, object>)ser.Deserialize(sr, typeof(Dictionary<string, object>));
            }

            // Check this point, I'm not sure if it's the best way to instantialize a specific type of Packet, 
            // I cannot instan it as base class because the base class is abstract
            switch (head.type)
            {
                case PacketType.Ping:
                    packet = new Ping(dataDict, head, serverClient); break;
                default:
                    packet = null; break;
            }
            return packet;
        }
    }
}