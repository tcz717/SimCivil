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
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyPacket"/> class.
        /// </summary>
        public EmptyPacket()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyPacket"/> class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data">dictionary storing data, consist of a string and a value</param>
        /// <param name="client">client indicating where to send to or received from</param>
        public EmptyPacket(PacketType type = PacketType.Empty, Hashtable data = null, IServerConnection client = null)
            : base(type, data, client)
        {
        }
    }
}