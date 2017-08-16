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
    public class ServerListener
    {
        TcpListener listener;
        public int Port { get; set; }
        public Queue<Packet> PacketReadQueue { get; set; }
        public Queue<Packet> PacketSendQueue { get; set; }
        public Dictionary<EndPoint, ServerClient> Clients { get; private set; } = new Dictionary<EndPoint, ServerClient>();

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

        public void SendPacket(Packet pkt)
        {
            lock (PacketSendQueue)
            {
                PacketSendQueue.Enqueue(pkt);
            }
        }

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
                    TcpClient currentClient = Task.Run(async () => await listener.AcceptTcpClientAsync()).Result;
                    ServerClient serverClient = new ServerClient(this, currentClient);
                    Clients.Add(currentClient.Client.RemoteEndPoint, serverClient);
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
    }
}
