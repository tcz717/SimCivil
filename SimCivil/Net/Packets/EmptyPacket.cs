using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// Can present any none-parameter packet
    /// </summary>
    [PacketType(PacketType.Empty)]
    [PacketType(PacketType.FullViewSync, LoginRequired = true)]
    [PacketType(PacketType.QueryRoleList, LoginRequired = true)]
    public class EmptyPacket : Packet
    {
    }
}
