using System;
using System.Collections.Generic;
using System.Net;

namespace SimCivil.Net
{
    public interface IServerListener
    {
        Dictionary<EndPoint, ServerClient> Clients { get; }
        void SendPacket(Packet pkt);
        void Start();

        /// <summary>
        /// The event triggered when a new ServerClient created
        /// </summary>
        event EventHandler<ServerClient> OnNewConnected;
        /// <summary>
        /// The event triggered when a connection closed
        /// </summary>
        event EventHandler<ServerClient> OnLostedConnection;
    }
}