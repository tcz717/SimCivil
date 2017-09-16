using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SimCivil.Net;
using SimCivil.Net.Packets;

namespace SimCivil.Test.VirtualClient
{
    class Client
    {
        private NetworkStream clientStream;
        private bool stopFlag = false;
        public int port;
        public Queue<Packet> receivedPackets = new Queue<Packet>();
        public Queue<Packet> PacketsForSend = new Queue<Packet>();

        public void Start(int pt)
        {
            port = pt;
            Task.Run(() =>
            {
                TcpClient client = new TcpClient();

                client.Connect(IPAddress.Loopback, port);

                clientStream = client.GetStream();

                Thread TRead = new Thread(ReadMessage);
                TRead.Start();

                while (true)
                {
                    if (stopFlag)
                    {
                        break;
                    }
                    
                    if (PacketsForSend.Count > 0)
                    {
                        byte[] data = PacketsForSend.Dequeue().ToBytes(1);
                        clientStream.Write(data, 0, data.Length);
                        clientStream.Flush();
                    }
                }
            });
        }

        public void Stop()
        {
            stopFlag = true;
        }

        void ReadMessage()
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
                    int lengthOfBody = clientStream.Read(buffer, 0, head.Length);
                    Packet pkt = PacketFactory.Create(null, head, buffer);

                    receivedPackets.Enqueue(pkt);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("A connection lost. Client running exception: " + e.Message);
                stopFlag = true;
            }
        }
    }
}
