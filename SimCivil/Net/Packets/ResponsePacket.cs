using System.Collections;
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
        public ResponsePacket(IServerConnection client, int refpacketID):base()
        {
            this.client = client;
            RefPacketID = refpacketID;
        }

        internal ResponsePacket(Hashtable data = null, IServerConnection client = null) : base(data, client)
        {
        }

        public override bool Verify()
        {
            return base.Verify()
                && Data.ContainsKey(nameof(RefPacketID))
                && Data[nameof(RefPacketID)] is int;
        }
    }
}