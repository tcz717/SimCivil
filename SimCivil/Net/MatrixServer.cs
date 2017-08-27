using log4net;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SimCivil.Config;

namespace SimCivil.Net
{
    public class MatrixServer : IServerListener
    {
        private const int Backlog = 50;
        static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ConcurrentDictionary<EndPoint, ServerClient> Clients { get; }

        public event EventHandler<IServerConnection> OnConnected;
        public event EventHandler<IServerConnection> OnDisconnection;
        CancellationTokenSource  cancellation = new CancellationTokenSource();
        /// <summary>
        /// Server host
        /// </summary>
        public IPAddress Host { get; }
        /// <summary>
        /// The port this listener listen to
        /// </summary>
        public int Port { get; set; }

        private readonly Socket socket;
        private readonly BufferManager buffer;
        private readonly BlockingCollection<Packet> PacketSendQueue;
        private readonly BlockingCollection<Packet> PacketReadQueue;
        private DefaultObjectPool<SocketAsyncEventArgs> socketArgsPool;
        private SocketAsyncEventArgs acceptArgs;

        /// <summary>
        /// Construct a serverlistener
        /// </summary>
        /// <param name="ip">ip to start listener</param>
        /// <param name="port">port to start listener</param>
        public MatrixServer(string ip,int port)
        {
            Host = IPAddress.Parse(ip);
            Port = port;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            buffer = BufferManager.CreateBufferManager(Packet.MaxSize * 1000, Packet.MaxSize);

            PacketSendQueue = new BlockingCollection<Packet>();
            PacketReadQueue = new BlockingCollection<Packet>();

            acceptArgs = new SocketAsyncEventArgs();
            acceptArgs.Completed += Accept_Completed;
        }

        public void SendPacket(Packet pkt)
        {
            throw new NotImplementedException();
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
            socketArgsPool = new DefaultObjectPool<SocketAsyncEventArgs>(
                new DefaultPooledObjectPolicy<SocketAsyncEventArgs>());

            socket.Bind(new IPEndPoint(Host,Port));
            socket.Listen(Backlog);
            StartAccept();
        }

        private void StartAccept()
        {
            acceptArgs.AcceptSocket = null;
            if (!socket.AcceptAsync(acceptArgs))
                ProcessAccept(acceptArgs);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                var client = new MatrixConnection(this, e.AcceptSocket);

                OnConnected?.Invoke(this, client);

                SocketAsyncEventArgs readArgs = socketArgsPool.Get();
                readArgs.UserToken = client;
                readArgs.Completed = ReadHead_Completed;
                readArgs.SetBuffer(buffer.TakeBuffer(Head.HeadLength),0,Head.HeadLength);

                if (e.AcceptSocket != null && !e.AcceptSocket.ReceiveAsync(readArgs))
                    ProcessReceive(readArgs);
            }
            e.AcceptSocket = null;
            this.StartAccept();
        }

        private void ReadHead_Completed(object sender, SocketAsyncEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Accept_Completed(object sender, SocketAsyncEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            cancellation.Cancel();
        }

        public void AttachClient(ServerClient client)
        {
            throw new NotImplementedException();
        }

        public void DetachClient(ServerClient client)
        {
            throw new NotImplementedException();
        }
    }
}
