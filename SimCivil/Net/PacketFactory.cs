using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using SimCivil.Net.Packets;

namespace SimCivil.Net
{
    public class PacketFactory
    {
        public static Packet Create(ServerClient serverClient, Head head, byte[] data)
        {
            JsonSerializer ser = new JsonSerializer();
            Packet packet;
            using (MemoryStream stream = new MemoryStream())
            {
                TextReader tw = new StreamReader(stream);
                stream.Write(data, 0, head.length);
                Dictionary<string, object> dataDict 
                    = (Dictionary<string, object>)ser.Deserialize(tw, typeof(Dictionary<string, object>));

                // Check this point, I'm not sure if it's the best way to instantialize a specific type of Packet, 
                // I cannot instan it as base class because the base class is abstract
                switch (head.type)
                {
                    case PacketType.Ping:
                        packet = new Ping(dataDict, head, serverClient); break;
                    default:
                        packet = null; break;
                }
            }
            return packet;
        }
    }
}