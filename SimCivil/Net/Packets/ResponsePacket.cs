using System.Collections.Generic;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// Packet that need to reply another Packet.
    /// </summary>
    public abstract class ResponsePacket : Packet
    {
        /// <summary>
        /// Ping packet's id need to response.
        /// </summary>
        public int RefPacketID
        {
            get
            {
                return (int)Data[nameof(RefPacketID)];
            }
            set
            {
                Data[nameof(RefPacketID)] = value;
            }
        }
        /// <summary>
        /// Construct a new ResponsePacket Packet.
        /// </summary>
        /// <param name="client">Client to response.</param>
        /// <param name="refpacketID">Requesting packet's id.</param>
        public ResponsePacket(ServerClient client, int refpacketID):base()
        {
            this.client = client;
            RefPacketID = refpacketID;
        }

        internal ResponsePacket(Dictionary<string, object> data = null, ServerClient client = null) : base(data, client)
        {
        }
    }
}