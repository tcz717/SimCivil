using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SimCivil.Net
{
    public abstract class Packet
    {
        public const int MaxSize = 4096; //单个数据包最大限制

        Dictionary<string, object> data;
        Head head;
        ServerClient client;

        public Dictionary<string, object> Data { get { return data; } set { data = value; } }
        public Head Head { get { return head; } set { head = value; } }
        public ServerClient Client { get { return client; } set { client = value; } }

        public Packet(Dictionary<string, object> data, Head head = default(Head), ServerClient client = null)
        {
            this.head = head;
            this.data = data;
            this.client = client;
        }

        public virtual void Send()
        {
            client.SendPacket(this);
        }

        /// <summary>
        /// The method executed after clients received and pushed in the PacketReadQueue
        /// </summary>
        public abstract void Handle();
        
        /// <summary>
        /// Update length stored in head and convert Packet, including head, to bytes
        /// </summary>
        /// <returns>bytes converted from Packet</returns>
        public virtual byte[] ToBytes()
        {
            JsonSerializer ser = new JsonSerializer();
            byte[] dataBytes;
            List<byte> bytes = new List<byte>();
            MemoryStream stream = new MemoryStream();
            using (StreamWriter tw = new StreamWriter(stream))
            {
                ser.Serialize(tw, data, typeof(Dictionary<string, object>));
            }
            dataBytes = stream.ToArray();

            head.length = dataBytes.Length;
            bytes.AddRange(head.ToBytes());
            bytes.AddRange(dataBytes);

            return bytes.ToArray();
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

        /// <summary>
        /// Build head from bytes
        /// </summary>
        /// <param name="buffer">buffer to build head</param>
        /// <returns>a brand new head</returns>
        public static Head FromBytes(byte[] buffer)
        {
            int packageID = BitConverter.ToInt32(buffer, 0);
            PacketType type = (PacketType)BitConverter.ToInt32(buffer, 4);
            int length = BitConverter.ToInt32(buffer, 8);

            return new Head(packageID, type, length);
        }

        /// <summary>
        /// Convert head to bytes for sending, Note: this method has been used in Head.ToBytes() for building packet bytes
        /// </summary>
        /// <returns>bytes converted from head</returns>
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
