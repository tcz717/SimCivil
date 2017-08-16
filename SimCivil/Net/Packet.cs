using System;
using System.Collections.Generic;
using System.Text;

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

        public virtual byte[] ToBytes()
        {
            throw new NotImplementedException();
        }
    }

    public struct Head
    {
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

        public static int Size { get; internal set; }

        public static Head FromBytes(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public byte[] ToBytes()
        {
            throw new NotImplementedException();
        }
    }
}
