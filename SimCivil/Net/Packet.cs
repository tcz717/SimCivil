using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SimCivil.Net
{
    public abstract class Packet
    {
        public const int MaxSize = 4096; //单个数据包最大限制

        public Dictionary<string,object> Data { get; set; }
        public Head Head { get; set; }
        public ServerClient Client { get; set; }

        public Packet(Dictionary<string, object> data, Head head = default(Head), ServerClient client = null)
        {
            Head = head;
            Data = data;
            Client = client;
        }

        public virtual void Send()
        {
            Client.SendPacket(this);
        }

        public abstract void Handle();

        // Not tested
        public virtual byte[] ToBytes()
        {
            JsonSerializer ser = new JsonSerializer();
            byte[] bytes;
            using (MemoryStream stream = new MemoryStream())
            {
                TextWriter tw = new StreamWriter(stream);
                ser.Serialize(tw, Data, typeof(Dictionary<string, object>));
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }

    public struct Head
    {
        public const int HeadLength = 12;
        public int packageID;
        public PacketType type;
        public int length;

        public Head(PacketType type) : this(0, type, 0) { }

        public Head(int packageID, PacketType type) : this(packageID, type, 0) { }

        public Head(int packageID, PacketType type, int length)
        {
            this.packageID = packageID;
            this.length = length;
            this.type = type;
        }

        public static Head FromBytes(byte[] buffer)
        {
            //TODO
            throw new NotImplementedException();
        }

        public byte[] ToBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(packageID));
            bytes.AddRange(BitConverter.GetBytes((int)type));
            bytes.AddRange(BitConverter.GetBytes(length));

            return bytes.ToArray();
        }
    }
}
