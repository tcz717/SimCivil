using System;
using System.Collections.Concurrent;
using System.Net;
using SimCivil.Net.Packets;

namespace SimCivil.Net
{
    /// <summary>
    /// Listener of server
    /// </summary>
    public interface IServerListener
    {
        /// <summary>
        /// Sends the packet.
        /// </summary>
        /// <param name="pkt">The PKT.</param>
        void SendPacket(Packet pkt);

        /// <summary>
        /// Starts this server.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this server.
        /// </summary>
        void Stop();

        /// <summary>
        /// Attaches the client.
        /// </summary>
        /// <param name="client">The client.</param>
        void AttachClient(IServerConnection client);

        /// <summary>
        /// Detaches the client.
        /// </summary>
        /// <param name="client">The client.</param>
        void DetachClient(IServerConnection client);

        /// <summary>
        /// Registers the packet.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="callBack">The call back.</param>
        void RegisterPacket(PacketType type, PacketCallBack callBack);

        /// <summary>
        /// Unregisters the packet.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="callBack">The call back.</param>
        void UnregisterPacket(PacketType type, PacketCallBack callBack);

        /// <summary>
        /// The event triggered when a new ServerClient created
        /// </summary>
        event EventHandler<IServerConnection> OnConnected;

        /// <summary>
        /// The event triggered when a connection closed
        /// </summary>
        event EventHandler<IServerConnection> OnDisconnected;
    }

    /// <summary>
    /// Function will be called after received a packet.
    /// </summary>
    /// <param name="pkt">The packet.</param>
    /// <param name="isVaild">if set to <c>true</c> [is vaild].</param>
    public delegate void PacketCallBack(Packet pkt, ref bool isVaild);
}