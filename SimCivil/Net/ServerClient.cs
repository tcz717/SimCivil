using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

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

        /// <summary>
        /// a flag indication if a client is trying to stop
        /// </summary>
        public bool stopFlag = false;

        /// <summary>
        /// The TcpClient used in this ServerClient
        /// </summary>
        public TcpClient TcpClt
        {
            get { return currentClient; }
            private set { }
        }

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
        }


        /// <summary>
        /// A method for send
        /// </summary>
        public void SendPacket(Packet pkt)
        {
            try
            {
                byte[] data = pkt.ToBytes();
                clientStream.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("Client connecting failed in Send: " + e.Message);
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
                            break;
                        }

                        byte[] buffer = new byte[Packet.MaxSize];
                        int lengthOfHead = clientStream.Read(buffer, 0, Head.HeadLength);
                        Head head = Head.FromBytes(buffer);
                        int lengthOfBody = clientStream.Read(buffer, 0, head.length);
                        Packet pkt = PacketFactory.Create(this, head, buffer);

                        serverListener.PushPacket(pkt);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("A connection lost. Client running exception: " + e.Message);
                    serverListener.StopAndRemoveClient(this);
                }
            });
        }
    }
}
