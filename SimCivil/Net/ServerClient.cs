using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using SimCivil.Net.Packets;
using SimCivil.Net;
using static SimCivil.Config;

namespace SimCivil.Net
{
    /// <summary>
    /// Clients created by listener
    /// </summary>
    public class ServerClient
    {
        private ServerListener serverListener;
        private TcpClient currentClient;
        private NetworkStream clientStream;
        private DateTime lastReceive;
        private int currentID;
        private bool isStart = false; // For TimeOutCheck
        private bool stopFlag = false; // For stop thread

        /// <summary>
        /// Register a callback when specfic packet received.
        /// </summary>
        /// <typeparam name="T">Packet type expected to receive.</typeparam>
        /// <param name="packet">Packet requesting callback.</param>
        public void WaitFor<T>(Packet packet) where T : ResponsePacket
        {
            // TODO @panyz522 add packet and response to dic
        }
        /// <summary>
        /// Event invoking when a packet received.
        /// </summary>
        public event EventHandler<Packet> OnPacketReceived;

        /// <summary>
        /// The TcpClient used in this ServerClient
        /// </summary>
        public TcpClient TcpClt
        {
            get { return currentClient; }
            private set { }
        }

        /// <summary>
        /// This Client's holder.
        /// </summary>
        public ServerListener ServerListener { get => serverListener; set => serverListener = value; }

        /// <summary>
        /// Construct a ServerClient for receiving Packets
        /// </summary>
        /// <param name="serverListener">the ServerListener creading this ServerClient</param>
        /// <param name="currentClient">the TcpClient used in this ServerClient</param>
        public ServerClient(ServerListener serverListener, TcpClient currentClient)
        {
            this.serverListener = serverListener;
            this.currentClient = currentClient;
            clientStream = currentClient.GetStream();
            currentID = 0;
        }

        /// <summary>
        /// Update packet head, convert packet to bytes, and send them.
        /// Note: this method should only be called by server and packet itself, please do NOT use it directly!
        /// It is recommended to enqueue packets into PacketSendQueue for sending
        /// </summary>
        public void SendPacket(Packet pkt)
        {
            try
            {
                byte[] data = pkt.ToBytes(currentID);
                clientStream.Write(data, 0, data.Length);
                ServerListener.logger.Debug($"Packet has been sent: \"ID:{pkt.Head.packetID} type:{pkt.Head.type}\"");

                if (++currentID >= Int32.MaxValue)
                {
                    currentID = 0;
                }
            }
            catch (Exception e)
            {
                ServerListener.logger.Error("Client connecting failed in Send: " + e.Message); 
            }
        }

        /// <summary>
        /// Start the client
        /// </summary>
        public void Start()
        {
            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        if (stopFlag)
                        {
                            clientStream.Dispose();
                            isStart = false;
                            break;
                        }

                        // Build packet
                        byte[] buffer = new byte[Packet.MaxSize];
                        int lengthOfHead = clientStream.Read(buffer, 0, Head.HeadLength);
                        Head head = Head.FromBytes(buffer);
                        int lengthOfBody = clientStream.Read(buffer, 0, head.length);
                        Packet pkt = PacketFactory.Create(this, head, buffer);

                        // Enqueue packet
                        serverListener.PushPacket(pkt);

                        // Packet receive action
                        lastReceive = DateTime.Now;
                        OnPacketReceived?.Invoke(this, pkt);

                        // Change Client status
                        isStart = true;
                    }
                }
                catch(Exception e)
                {
                    ServerListener.logger.Info($"Client \"{TcpClt.Client.RemoteEndPoint}\" failed to receive packet and forced to stop. Exception: {e.Message}");
                    serverListener.StopAndRemoveClient(this);
                }
            });
        }

        /// <summary>
        /// Stop client.
        /// </summary>
        public void Stop()
        {
            stopFlag = true;
            ServerListener.logger.Debug($"Client \"{TcpClt.Client.RemoteEndPoint}\" is flagged to stop");
        }

        /// <summary>
        /// Check whether the client has not received message for a while and determine what to do
        /// </summary>
        public void TimeOutCheck()
        {
            if (isStart)
            {
                if (DateTime.Now - lastReceive > PingRequestTime)
                {
                    serverListener.SendPacket(new Ping());
                    ServerListener.logger.Info($"Client \"{TcpClt.Client.RemoteEndPoint}\" sent Ping request due to time out");
                }
                if (DateTime.Now - lastReceive > LostConnectionTime)
                {
                    ServerListener.logger.Info($"Client \"{TcpClt.Client.RemoteEndPoint}\" receiving out of time, and ordered to stop");
                    serverListener.StopAndRemoveClient(this);
                }
            }
        }
    }
}
