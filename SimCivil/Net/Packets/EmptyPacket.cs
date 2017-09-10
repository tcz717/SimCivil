using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// Can present any none-parameter packet
    /// </summary>
    [PacketType(PacketType.FullViewSync, LoginRequired = true)]
    [PacketType(PacketType.QueryRoleList, LoginRequired = true)]
    public class EmptyPacket : Packet
    {
        public EmptyPacket() { }
        public EmptyPacket(PacketType type = PacketType.Empty, Hashtable data = null, IServerConnection client = null)
            : base(type, data, client) { }
    }
}
