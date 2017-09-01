using log4net;
using SimCivil.Net.Packets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SimCivil.Config;

namespace SimCivil.Net
{
    public class MatrixServer : IServerListener, ITicker
    {
        static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ConcurrentDictionary<EndPoint, IServerConnection> Clients { get; }

        private ConcurrentDictionary<PacketType, PacketCallBack> callbackDict;

        public event EventHandler<IServerConnection> OnConnected;
        public event EventHandler<IServerConnection> OnDisconnected;
        CancellationTokenSource cancellation = new CancellationTokenSource();
        /// <summary>
        /// Server host
        /// </summary>
        public IPAddress Host { get; }
        /// <summary>
        /// The port this listener listen to
        /// </summary>
        public int Port { get; set; }
        public int Priority { get; } = 900;

        private readonly Socket socket;
        private readonly BlockingCollection<Packet> PacketSendQueue;
        private readonly BlockingCollection<Packet> PacketReadQueue;

        /// <summary>
        /// Construct a serverlistener
        /// </summary>
        /// <param name="ip">ip to start listener</param>
        /// <param name="port">port to start listener</param>
        public MatrixServer(string ip, int port)
        {
            Host = IPAddress.Parse(ip);
            Port = port;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            PacketSendQueue = new BlockingCollection<Packet>();
            PacketReadQueue = new BlockingCollection<Packet>();

            Clients = new ConcurrentDictionary<EndPoint, IServerConnection>();

            callbackDict = new ConcurrentDictionary<PacketType, PacketCallBack>(
                PacketFactory.LegalPackets.Select(lp => new KeyValuePair<PacketType, PacketCallBack>(lp.Key, null))
               );
        }

        public void SendPacket(Packet pkt)
        {
            pkt.Client.SendPacket(pkt);
        }

        public void Start()
        {
            Task.Factory.StartNew(
                () => LisenteningLoop(cancellation.Token),
                cancellation.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Current);
        }

        private void LisenteningLoop(CancellationToken token)
        {
            Thread.CurrentThread.Name = nameof(LisenteningLoop);
            TcpListener listener = new TcpListener(Host, Port);
            listener.Start();
            logger.Info($"{nameof(MatrixServer)} start at {Host}:{Port}");
            while (!cancellation.IsCancellationRequested)
            {
                var socket = listener.AcceptSocket();
                var con = new MatrixConnection(this, socket);
                AttachClient(con);
                var _ = ReadPacketAsync(con);
            }
            listener.Stop();
        }

        private async Task ReadPacketAsync(MatrixConnection con)
        {
            byte[] buffer = new byte[Packet.MaxSize];
            int lengthOfBody = 0;
            try
            {
                while (!cancellation.Token.IsCancellationRequested)
                {
                    int lengthOfHead = await con.Stream.ReadAsync(buffer, 0, Head.HeadLength);
                    Head head = Head.FromBytes(buffer);
                    lengthOfBody = await con.Stream.ReadAsync(buffer, 0, head.length);

                    Packet pkt = PacketFactory.Create(con, head, buffer);

                    logger.Debug($"received packet {pkt}");
                    PacketReadQueue.Add(pkt);
                }
            }
            catch (Exception e)
            {
                byte[] body = new byte[lengthOfBody];
                Array.Copy(buffer, body, lengthOfBody);
                logger.Error($"{nameof(ReadPacketAsync)} read error packet {BitConverter.ToString(body)}", e);
            }
            finally
            {
                con.Close();
            }
        }

        public void Stop()
        {
            cancellation.Cancel();
            foreach (var c in Clients.Values)
            {
                c.Close();
            }
        }

        public void AttachClient(IServerConnection client)
        {
            MatrixConnection connection = client as MatrixConnection ?? throw new NotSupportedException();
            if (!Clients.TryAdd(connection.Socket.RemoteEndPoint, client))
                throw new ArgumentException();
            OnConnected?.Invoke(this, client);
            logger.Info($"Connection established {connection.Socket.RemoteEndPoint}");
        }

        public void DetachClient(IServerConnection client)
        {
            MatrixConnection connection = client as MatrixConnection ?? throw new NotSupportedException();
            Clients.Remove(connection.Socket.RemoteEndPoint, out client);
            OnDisconnected?.Invoke(this, client);
            logger.Info($"Detached Connection {connection.Socket.RemoteEndPoint}");
        }

        public void Update(int tickCount)
        {
            while(PacketReadQueue.TryTake(out Packet pkt))
            {
                bool isVaild = pkt.Verify();
                if (isVaild)
                {
                    pkt.ReplyError(desc: "Packet format invaild");
                    continue;
                }
                callbackDict[pkt.Head.type]?.Invoke(pkt, ref isVaild);
                if (isVaild)
                    pkt.Handle();
            }
        }

        public void RegisterPacket(PacketType type, PacketCallBack callBack)
        {
            callbackDict[type] += callBack;
        }

        public void UnregisterPacket(PacketType type, PacketCallBack callBack)
        {
            callbackDict[type] -= callBack;
        }
    }
}
