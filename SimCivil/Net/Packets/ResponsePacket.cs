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
        public int RefPacketId
        {
            get => (int)Data[nameof(RefPacketId)];
            set => Data[nameof(RefPacketId)] = value;
        }
        /// <summary>
        /// Construct a new ResponsePacket Packet.
        /// </summary>
        /// <param name="client">Client to response.</param>
        /// <param name="refpacketID">Requesting packet's id.</param>
        protected ResponsePacket(IServerConnection client, int refpacketID):base()
        {
            this.Client = client;
            RefPacketId = refpacketID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponsePacket"/> class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data">dictionary storing data, consist of a string and a value</param>
        /// <param name="client">client indicating where to send to or received from</param>
        protected ResponsePacket(PacketType type = PacketType.Empty, Hashtable data = null, IServerConnection client = null)
            : base(type, data, client)
        {
        }

        /// <summary>
        /// Verify this packet's receiving correctness.
        /// </summary>
        /// <param name="errorDesc"></param>
        /// <returns></returns>
        public override bool Verify(out string errorDesc)
        {
            return base.Verify(out errorDesc)
                && Data.ContainsKey(nameof(RefPacketId))
                && Data[nameof(RefPacketId)] is int;
        }
    }
}