using log4net;
using System;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using SimCivil.Net.Packets;
using SimCivil.Auth;

namespace SimCivil.Net
{
    public class MatrixConnection : IServerConnection, IDisposable
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private int currentID = 0;

        public MatrixConnection(IServerListener serverListener, Socket socket)
        {
            ServerListener = serverListener;
            Socket = socket;
            Stream = new NetworkStream(socket);
        }

        public IServerListener ServerListener { get; set; }
        public Socket Socket { get; }
        public NetworkStream Stream { get; }
        public bool Connected => Socket.Connected;

        public Player ContextPlayer { get; set; }

        public event EventHandler<Packet> OnPacketReceived;
        public event EventHandler OnDisconnected;

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

        public void Dispose()
        {
            Stream?.Dispose();
            Socket?.Dispose();
        }

        public void SendPacket(Packet pkt)
        {
            if (!Socket.Connected)
                throw new InvalidOperationException("Socket closed.");

            byte[] data = pkt.ToBytes(currentID);
            var send = Stream.WriteAsync(data, 0, data.Length);
            if (pkt is ErrorResponse)
                send.ContinueWith(t => Close());
            logger.Debug($"Packet has been sent: \"ID:{pkt.Head.packetID} type:{pkt.Head.type}\"");

            Interlocked.Increment(ref currentID);
        }

        public void TimeOutCheck()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Used to send a packet and request it's reply for next step.
        /// </summary>
        /// <typeparam name="T">ResponsePacket</typeparam>
        /// <param name="packet">Packet to send</param>
        /// <param name="callback">callback wehen received specified response.</param>
        public void SendAndWait<T>(Packet packet, Action<T> callback) where T : ResponsePacket
        {
            var type = PacketFactory.PacketsType[typeof(T)];
            int tries = 50;
            packet.Client = this;
            void temp_callback(Packet pkt, ref bool vaild)
            {
                T re = pkt as T;
                if (re.RefPacketID == packet.Head.packetID && vaild)
                {
                    callback(re);
                    ServerListener.UnregisterPacket(type, temp_callback);
                }
                else if (--tries < 0)
                    ServerListener.UnregisterPacket(type, temp_callback);
            }
            ServerListener.RegisterPacket(type, temp_callback);
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

        ~MatrixConnection()
        {
            Dispose();
        }
    }
}