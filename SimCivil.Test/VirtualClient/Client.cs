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
        private static NetworkStream clientStream;
        private static bool stopFlag = false;
        public static int port;
        public static Queue<Packet> receivedPackets = new Queue<Packet>();
        public static Queue<Packet> PacketsForSend = new Queue<Packet>();

        public static void Start(int pt)
        {
            port = pt;
            Task.Run(() =>
            {
                TcpClient client = new TcpClient();

                client.Connect(IPAddress.Parse("127.0.0.1"), port);

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
                        byte[] data = PacketsForSend.Dequeue().ToBytes();
                        clientStream.Write(data, 0, data.Length);
                    }
                }
            });
        }

        public static void Stop()
        {
            stopFlag = true;
        }

        static void ReadMessage()
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
