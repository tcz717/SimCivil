using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SimCivil.Test.VirtualClient
{
    class Client
    {
        private static NetworkStream clientStream;
        public static int port;

        public static void Start(int pt)
        {
            port = pt;
            Task.Run(() =>
            {
                TcpClient client = new TcpClient();

                client.ConnectAsync(IPAddress.Parse("127.0.0.1"), port);

                clientStream = client.GetStream();
                StreamWriter sw = new StreamWriter(clientStream);

                Thread TRead = new Thread(ReadMessage);
                TRead.Start();

                while (true)
                {
                    string input = Console.ReadLine();

                    if (input.Equals("clientexit"))
                    {
                        break;
                    }
                    sw.WriteLine(input);
                    sw.Flush();
                }
            });
        }

        static void ReadMessage()
        {
            StreamReader sr = new StreamReader(clientStream);
            Console.WriteLine("Client start read...");
            while (true)
            {
                string rcv;
                try
                {
                    rcv = sr.ReadLine();
                }
                catch (Exception e)
                {
                    string exceptionInfo = e.Message;
                    break;
                }

                Console.WriteLine(rcv);
            }
        }
    }
}
