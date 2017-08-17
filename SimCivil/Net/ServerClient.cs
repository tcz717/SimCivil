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

        public bool stopFlag = false;

        public TcpClient TcpClt
        {
            get { return currentClient; }
            private set { }
        }

        public ServerClient(ServerListener serverListener, TcpClient currentClient)
        {
            this.serverListener = serverListener;
            this.currentClient = currentClient;
            clientStream = currentClient.GetStream();
        }


        /// <summary>
        /// A method for sending thread
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

        public void Start()
        {
            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        if (stopFlag)
                            break;

                        byte[] buffer = new byte[Packet.MaxSize];
                        int size = clientStream.Read(buffer, 0, Head.HeadLength);
                        Head head = Head.FromBytes(buffer);
                        size = clientStream.Read(buffer, 0, head.length);
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
