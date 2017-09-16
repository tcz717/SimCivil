using System.Collections;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// A Ping packet's response.
    /// </summary>
    [PacketType(PacketType.PingResponse)]
    public class PingResponse : ResponsePacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PingResponse"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="data">The data.</param>
        /// <param name="client">The client.</param>
        public PingResponse(PacketType type = PacketType.Empty, Hashtable data = null, IServerConnection client = null)
            : base(type, data, client)
        {
        }
    }
}