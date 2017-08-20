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
        /// <summary>
        /// Construct a Ping Packet
        /// </summary>
        /// <param name="data">dictionary storing data, consist of a string and a value</param>
        /// <param name="head">head storing ID, type, and body length</param>
        /// <param name="client">client indicating where to send to or received from</param>
        public Ping(Dictionary<string, object> data, Head head = default(Head), ServerClient client = null) : base(data, head, client)
        {
        }

        /// <summary>
        /// Ping Event triggered after server received a ping request
        /// </summary>
        public static event Action<Dictionary<string, object>> PingEvent;

        /// <summary>
        /// Throw a Ping event
        /// </summary>
        public override void Handle() 
        {
            PingEvent(data);
        }
    }
}
