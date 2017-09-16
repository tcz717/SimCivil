using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// A type of Packet for ping
    /// </summary>
    [PacketType(PacketType.Ping)]
    public class Ping : Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ping"/> class.
        /// </summary>
        public Ping()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ping"/> class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data">dictionary storing data, consist of a string and a value</param>
        /// <param name="client">client indicating where to send to or received from</param>
        public Ping(PacketType type, Hashtable data, IServerConnection client) : base(type, data, client)
        {
        }

        /// <summary>
        /// Ping handle
        /// </summary>
        public override void Handle()
        {
            Reply(new PingResponse());
        }
    }
}