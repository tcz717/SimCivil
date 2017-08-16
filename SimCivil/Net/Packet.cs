using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Net
{
    public class Packet
    {
        public Dictionary<string,object> Data { get; set; }
        public Head Head { get; set; }

        public Packet(Dictionary<string, object> data) : this(new Head(0), data) { }

        public Packet(Head head, Dictionary<string, object> data)
        {
            Head = head;
            Data = data;
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
    }
}
