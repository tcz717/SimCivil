using SimCivil.Auth;
using SimCivil.Net.Packets;
using System;

namespace SimCivil.Net
{
    /// <summary>
    /// A Server connection
    /// </summary>
    public interface IServerConnection
    {
        /// <summary>
        /// Gets or sets the server listener.
        /// </summary>
        /// <value>
        /// The server listener.
        /// </value>
        IServerListener ServerListener { get; set; }

        /// <summary>
        /// Gets or sets the context player.
        /// </summary>
        /// <value>
        /// The context player.
        /// </value>
        Player ContextPlayer { get; set; }

        /// <summary>
        /// Occurs when [on disconnected].
        /// </summary>
        event EventHandler OnDisconnected;

        /// <summary>
        /// Occurs when [on packet received].
        /// </summary>
        [Obsolete]
        event EventHandler<Packet> OnPacketReceived;

        /// <summary>
        /// Times the out check.
        /// </summary>
        void TimeOutCheck();

        /// <summary>
        /// Sends the packet.
        /// </summary>
        /// <param name="pkt">The PKT.</param>
        void SendPacket(Packet pkt);

        /// <summary>
        /// Closes this connection.
        /// </summary>
        void Close();

        /// <summary>
        /// Gets a value indicating whether this <see cref="IServerConnection"/> is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if connected; otherwise, <c>false</c>.
        /// </value>
        bool Connected { get; }

        /// <summary>
        /// Used to send a packet and request it's reply for next step.
        /// </summary>
        /// <typeparam name="T">ResponsePacket</typeparam>
        /// <param name="packet">Packet to send</param>
        /// <param name="callback">callback wehen received specified response.</param>
        void SendAndWait<T>(Packet packet, Action<T> callback) where T : ResponsePacket;
    }
}