using System;
using System.Collections.Concurrent;
using System.Net;

namespace SimCivil.Net
{
    public interface IServerListener
    {
        ConcurrentDictionary<EndPoint, ServerClient> Clients { get; }
        void SendPacket(Packet pkt);
        void Start();
        void Stop();
        void AttachClient(ServerClient client);
        void DetachClient(ServerClient client);

        /// <summary>
        /// The event triggered when a new ServerClient created
        /// </summary>
        event EventHandler<IServerConnection> OnConnected;
        /// <summary>
        /// The event triggered when a connection closed
        /// </summary>
        event EventHandler<IServerConnection> OnDisconnection;
    }
}