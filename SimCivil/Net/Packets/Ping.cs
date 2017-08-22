using System;
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
        public Ping(Dictionary<string, object> data = null, ServerClient client = null) : base(data, client)
        {
        }

        /// <summary>
        /// Ping handle
        /// </summary>
        public override void Handle() 
        {
            Client.ServerListener.SendPacket(new PingResponse(Client, head.packetID));
            Client.WaitFor<PingResponse>(this); // TODO: why here?
        }
    }
}
