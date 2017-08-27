using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using SimCivil;
using SimCivil.Net.Packets;
using System.Collections.Concurrent;

namespace SimCivil.Net
{
    /// <summary>
    /// Control TcpListener and a Listener Thread
    /// </summary>
    public class ServerListener : IServerListener, ITicker
    {
        TcpListener listener;

        /// <summary>
        /// Logger for net
        /// </summary>
        public static readonly ILog logger = LogManager.GetLogger(typeof(ServerListener));
        /// <summary>
        /// Server host
        /// </summary>
        public IPAddress Host { get; }
        /// <summary>
        /// The port this listener listen to
        /// </summary>
        public int Port { get;  set; }
        /// <summary>
        /// Packets received from ServerClients and waiting for handling
        /// </summary>
        private Queue<Packet> PacketReadQueue { get; set; }
        /// <summary>
        /// Packets waiting for sending
        /// </summary>
        private  Queue<Packet> PacketSendQueue { get; set; }
        /// <summary>
        /// ServerClients which are communicating with other clients
        /// </summary>
        public ConcurrentDictionary<EndPoint, ServerClient> Clients { get; private set; } = new ConcurrentDictionary<EndPoint, ServerClient>();

        /// <summary>
        /// The event triggered when a new ServerClient created
        /// </summary>
        public event EventHandler<IServerConnection> OnConnected;
        /// <summary>
        /// The event triggered when a connection closed
        /// </summary>
        public event EventHandler<IServerConnection> OnDisconnection;

        /// <summary>
        /// Construct a serverlistener
        /// </summary>
        /// <param name="host">server host</param>
        /// <param name="port">port to start listener</param>
        public ServerListener(IPAddress host, int port)
        {
            Host = host;
            Port = port;
            PacketSendQueue = new Queue<Packet>();
            PacketReadQueue = new Queue<Packet>();
        }
        /// <summary>
        /// Construct a serverlistener
        /// </summary>
        /// <param name="port">port to start listener</param>
        public ServerListener(int port)
        {
            Host = IPAddress.Loopback;
            Port = port;
            PacketSendQueue = new Queue<Packet>();
            PacketReadQueue = new Queue<Packet>();
        }

        /// <summary>
        /// Start the listener
        /// </summary>
        public void Start()
        {
            Task.Factory.StartNew(ListeningHandle ,TaskCreationOptions.AttachedToParent);
            Task.Factory.StartNew(SendWorker, TaskCreationOptions.AttachedToParent);
            logger.Info($"ServerListnener registered at port: {Port}");
        }

        /// <summary>
        /// Stop the client thread and remove it from storage
        /// </summary>
        /// <param name="client">the client to be removed</param>
        /// <returns></returns>
        public bool StopAndRemoveClient(ServerClient client)
        {
            EndPoint endPoint = client.TcpClt.Client.RemoteEndPoint;
            if (Clients.TryRemove(endPoint, out var c) && c == client)
            {
                client.Stop();
                OnDisconnection?.Invoke(this, client);
                logger.Info($"Client to \"{endPoint}\" stopped");
                return true;
            }
            logger.Error($"Failed to stop client. Client \"{client.TcpClt.Client.RemoteEndPoint}\" has not been registered correctly");
            return false;
        }

        /// <summary>
        /// Enqueue the packet and wait for sending
        /// </summary>
        /// <param name="pkt">the packet to send</param>
        public void SendPacket(Packet pkt)
        {
            lock (PacketSendQueue)
            {
                PacketSendQueue.Enqueue(pkt);
            }
            logger.Debug($"Packet has been enqueued and is waiting for sending: \"type: {pkt.Head.type}\"");
        }

        /// <summary>
        /// Enqueue the packet and wait for handling
        /// </summary>
        /// <param name="pkt">the packet to handle</param>
        internal void PushPacket(Packet pkt)
        {
            lock (PacketReadQueue)
            {
                PacketReadQueue.Enqueue(pkt);
            }
            logger.Debug($"Packet has been enqueued and is waiting for reading: \"ID: {pkt.Head.packetID} type: {pkt.Head.type}\"");
        }

        private void SendWorker()
        {
            Thread.CurrentThread.Name = nameof(SendWorker);
            while (true)
            {
                Packet pkt = null;
                lock (PacketSendQueue)
                {
                    if (PacketSendQueue.Count > 0)
                        pkt = PacketSendQueue.Dequeue();
                }
                pkt?.Send();
                Thread.Yield();
            }
        }

        /// <summary>
        /// Listen to connection requests and create threads to handle the requests
        /// </summary>
        private void ListeningHandle()
        {
            Thread.CurrentThread.Name = nameof(ListeningHandle);
            try
            {
                listener = new TcpListener(IPAddress.Any, Port);
                listener.Start();
                logger.Info($"ServerListnener started at port: {Port}");

                while (true)
                {
                    TcpClient currentClient = listener.AcceptTcpClient();
                    ServerClient serverClient = new ServerClient(this, currentClient);
                    AttachClient(serverClient);
                    logger.Info($"Connection Established to {serverClient.TcpClt.Client.RemoteEndPoint}");
                }
            }
            catch(ArgumentOutOfRangeException e)
            {
                logger.Error("Cannot start TcpListener: " + e.Message);
            }
            catch(Exception e)
            {
                logger.Error("Listener exception: " + e.Message);
            }
            finally
            {
                listener?.Stop();
            }
        }

        /// <summary>
        /// Stop all clients thread and remove them from storage
        /// </summary>
        public bool StopAndRemoveAllClient()
        {
            bool result = true;
            var clients = Clients.Values;
            foreach (var c in clients)
            {
                result &= StopAndRemoveClient(c);
                OnDisconnection?.Invoke(this, c);
            }
            if (result == true)
                logger.Info("Clients have been cleaned");
            else
                logger.Error("Failed to remove all clients. Some clients may not be registered correctly");
            return result;
        }

        /// <summary>
        /// Handle all packets in ReadQueue.
        /// </summary>
        /// <param name="tickCount">Total tick.</param>
        public void Update(int tickCount)
        {
            while (true)
            {
                Packet pkt = null;
                // To reduce lock time
                lock (PacketReadQueue)
                {
                    if (PacketReadQueue.Count > 0)
                        pkt = PacketReadQueue.Dequeue();
                    else
                        break;
                }
                if (pkt != null)
                {
                    pkt.Handle();
                    if (pkt is ResponsePacket)
                    {
                        var srcPacket = pkt.Client.CallFor(pkt as ResponsePacket);
                        srcPacket.ResponseCallback(pkt);
                    }
                }

                foreach(var client in Clients.Values)
                {
                    client.TimeOutCheck();
                }
            }
        }

        public void Stop()
        {
            StopAndRemoveAllClient();
        }

        public void AttachClient(ServerClient client)
        {
            Clients.Add(client.TcpClt.Client.RemoteEndPoint, client);
            OnConnected?.Invoke(this, client);
            client.Start();
        }

        public void DetachClient(ServerClient client) => StopAndRemoveClient(client);
    }
}
