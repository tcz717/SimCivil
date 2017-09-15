using System.Collections;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// A Ping packet's response.
    /// </summary>
    [PacketType(PacketType.PingResponse)]
    public class PingResponse : ResponsePacket
    {
        public PingResponse(PacketType type = PacketType.Empty, Hashtable data = null, IServerConnection client = null)
            : base(type, data, client)
        {

        }
        /// <summary>
        /// Called when received and request client object to update timeout flag.
        /// </summary>
        public override void Handle()
        {
            // No need to handle here
        }
    }
}