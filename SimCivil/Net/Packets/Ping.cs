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
        public Ping(Hashtable data = null, IServerConnection client = null) : base(data, client)
        {
        }

        /// <summary>
        /// Ping handle
        /// </summary>
        public override void Handle() 
        {
            Client.ServerListener.SendPacket(new PingResponse(Client, head.packetID));
        }

        /// <summary>
        /// Send packet immediately. And wait for PingResponse.
        /// </summary>
        public override void Send()
        {
            base.Send();
        }
    }
}
