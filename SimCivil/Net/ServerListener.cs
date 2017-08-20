using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SimCivil;

namespace SimCivil.Net
{
    /// <summary>
    /// Control TcpListener and a Listener Thread
    /// </summary>
    public class ServerListener : IServerListener
    {
        TcpListener listener;
        public int Port { get; set; }
        public Queue<Packet> PacketReadQueue { get; set; }
        public Queue<Packet> PacketSendQueue { get; set; }
        public Dictionary<EndPoint, ServerClient> Clients { get; private set; } = new Dictionary<EndPoint, ServerClient>();
        public Dictionary<ServerClient,EndPoint> EndPoints { get; private set; } = new Dictionary<ServerClient, EndPoint>();

        /// <summary>
        /// Construct a serverlistener
        /// </summary>
        /// <param name="port">port to start listener</param>
        public ServerListener(int port)
        {
            Port = port;
            PacketSendQueue = new Queue<Packet>();
            PacketReadQueue = new Queue<Packet>();
        }


        /// <summary>
        /// Start the listener
        /// </summary>
        public void Start()
        {
            Task.Run(new Action(ListeningHandle));
            Task.Run(new Action(PushAndPollHandle));
        }

        /// <summary>
        /// Stop the client thread and remove it from storage
        /// </summary>
        /// <param name="client">the client to be removed</param>
        /// <returns></returns>
        public bool StopAndRemoveClient(ServerClient client)
        {
            if (Clients.ContainsValue(client))
            {
                client.stopFlag = true;
                return DeleteClientFromDict(client);
            }
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
        }

        private void PushAndPollHandle()
        {
            while (true)
            {
                Packet pkt = null;
                lock(PacketReadQueue)
                {
                    if (PacketReadQueue.Count > 0)
                        pkt = PacketReadQueue.Dequeue();
                }
                pkt?.Handle();

                pkt = null;
                lock (PacketSendQueue)
                {
                    if (PacketSendQueue.Count > 0)
                        pkt = PacketSendQueue.Dequeue();
                }
                pkt?.Send();
            }
        }

        /// <summary>
        /// Listen to connection requests and create threads to handle the requests
        /// </summary>
        private void ListeningHandle()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, Port);
                listener.Start();
                Console.WriteLine("Listener started");

                while (true)
                {
                    TcpClient currentClient = listener.AcceptTcpClient();
                    ServerClient serverClient = new ServerClient(this, currentClient);
                    AddClientToDict(serverClient);
                    serverClient.Start();
                    Console.WriteLine("A Connection Established");
                }
            }
            catch(ArgumentOutOfRangeException e)
            {
                Console.WriteLine("Cannot start TcpListener: " + e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine("Listener exception: " + e.Message);
            }
            finally
            {
                listener?.Stop();
            }
        }

        private void AddClientToDict(ServerClient serverClient)
        {
            Clients.Add(serverClient.TcpClt.Client.RemoteEndPoint, serverClient);
            EndPoints.Add(serverClient, serverClient.TcpClt.Client.RemoteEndPoint);
        }

        private bool DeleteClientFromDict(ServerClient serverClient)
        {
            bool result = false;
            result |= Clients.Remove(serverClient.TcpClt.Client.RemoteEndPoint);
            result |= EndPoints.Remove(serverClient);
            return result;
        }

        private bool DeleteClientFromDict(EndPoint endPoint)
        {
            return DeleteClientFromDict(Clients[endPoint]);
        }
    }
}
