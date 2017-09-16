using log4net;
using System;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using SimCivil.Net.Packets;
using SimCivil.Auth;

namespace SimCivil.Net
{
    /// <summary>
    /// An implement of IServerConnection with better performance.
    /// </summary>
    /// <seealso>
    ///     <cref>SimCivil.Net.IServerConnection</cref>
    /// </seealso>
    /// <seealso>
    ///     <cref>System.IDisposable</cref>
    /// </seealso>
    public class MatrixConnection : IServerConnection, IDisposable
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private int _currentId;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixConnection"/> class.
        /// </summary>
        /// <param name="serverListener">The server listener.</param>
        /// <param name="socket">The socket.</param>
        public MatrixConnection(IServerListener serverListener, Socket socket)
        {
            ServerListener = serverListener;
            Socket = socket;
            Stream = new NetworkStream(socket);
        }

        /// <summary>
        /// Gets or sets the server listener.
        /// </summary>
        /// <value>
        /// The server listener.
        /// </value>
        public IServerListener ServerListener { get; set; }

        /// <summary>
        /// Gets the socket.
        /// </summary>
        /// <value>
        /// The socket.
        /// </value>
        public Socket Socket { get; }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <value>
        /// The stream.
        /// </value>
        public NetworkStream Stream { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="MatrixConnection"/> is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if connected; otherwise, <c>false</c>.
        /// </value>
        public bool Connected => Socket.Connected;

        /// <summary>
        /// Gets or sets the context player.
        /// </summary>
        /// <value>
        /// The context player.
        /// </value>
        public Player ContextPlayer { get; set; }

        /// <summary>
        /// Occurs when [on packet received].
        /// </summary>
        public event EventHandler<Packet> OnPacketReceived;

        /// <summary>
        /// Occurs when [on disconnected].
        /// </summary>
        public event EventHandler OnDisconnected;

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            if (Connected)
            {
                Stream?.Close();
                OnDisconnected?.Invoke(this, new EventArgs());
                ServerListener.DetachClient(this);
                logger.Info($"Disconnected {Socket.RemoteEndPoint}");
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Stream?.Dispose();
            Socket?.Dispose();
        }

        /// <summary>
        /// Sends the packet.
        /// </summary>
        /// <param name="pkt">The PKT.</param>
        /// <exception cref="InvalidOperationException">Socket closed.</exception>
        public void SendPacket(Packet pkt)
        {
            if (!Socket.Connected)
                throw new InvalidOperationException("Socket closed.");

            byte[] data = pkt.ToBytes(_currentId);
            var send = Stream.WriteAsync(data, 0, data.Length);
            if (pkt is ErrorResponse)
                send.ContinueWith(t => Close());
            logger.Debug($"Packet has been sent: \"ID:{pkt.PacketHead.PacketId} type:{pkt.PacketHead.Type}\"");

            Interlocked.Increment(ref _currentId);
        }

        /// <summary>
        /// Times the out check.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void TimeOutCheck()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Used to send a packet and request it's reply for next step.
        /// </summary>
        /// <typeparam name="T">ResponsePacket</typeparam>
        /// <param name="packet">Packet to send</param>
        /// <param name="callback">callback when received specified response.</param>
        public void SendAndWait<T>(Packet packet, Action<T> callback) where T : ResponsePacket
        {
            var type = PacketFactory.PacketsType[typeof(T)];
            // ReSharper disable once TooWideLocalVariableScope
            int tries = 50;
            packet.Client = this;

            void TempCallback(Packet pkt, ref bool valid)
            {
                T re = pkt as T;
                if (re.RefPacketId == packet.PacketHead.PacketId && valid)
                {
                    callback(re);
                    ServerListener.UnregisterPacket(type, TempCallback);
                }
                else if (--tries < 0)
                    ServerListener.UnregisterPacket(type, TempCallback);
            }

            ServerListener.RegisterPacket(type, TempCallback);
            packet.Send();
        }

        /// <summary>
        /// Return remote EndPoint's string,
        /// </summary>
        /// <returns>RemoteEndPoint.ToString</returns>
        public override string ToString()
        {
            return Socket.RemoteEndPoint.ToString();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MatrixConnection"/> class.
        /// </summary>
        ~MatrixConnection()
        {
            Dispose();
        }
    }
}