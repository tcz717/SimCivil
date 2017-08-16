using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
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
    class SeverListener
    {
        TcpListener listener;
        Queue<Package> packageSendQueue;
        Queue<Package> packageReadQueue;
        int port;
        
        /// <summary>
        /// Construct a serverlistener
        /// </summary>
        /// <param name="port">port to start listener</param>
        /// <param name="packageSendQueue">a buffer of package for sending</param>
        /// <param name="packageReadQueue">a buffer of package for reading</param>
        public SeverListener(int port, Queue<Package> packageSendQueue, Queue<Package> packageReadQueue)
        {
            this.port = port;
            this.packageSendQueue = packageSendQueue;
            this.packageReadQueue = packageReadQueue;

            ServerClient._packageSendQueue = packageSendQueue;
            ServerClient._packageReadQueue = packageReadQueue;
        }

        /// <summary>
        /// Start the listener
        /// </summary>
        public void Start()
        {
            Thread TListen = new Thread(ListeningAsync);
            TListen.Start();
        }

        /// <summary>
        /// Listen to connection requests and create threads to handle the requests
        /// </summary>
        private async void ListeningAsync()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                Console.WriteLine("Listener started");

                while (true)
                {
                    TcpClient currentClient = await listener.AcceptTcpClientAsync();
                    ServerClient serverClient = new ServerClient(currentClient);
                    Thread TClientRead = new Thread(serverClient.ReadMessage);
                    Thread TClientSend = new Thread(serverClient.SendMessage);
                    TClientRead.Start();
                    TClientSend.Start();
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

    /// <summary>
    /// Clients created by listener
    /// </summary>
    class ServerClient
    {
        TcpClient client;
        public static Queue<Package> _packageSendQueue;
        public static Queue<Package> _packageReadQueue;

        /// <summary>
        /// Construct a serverclient
        /// </summary>
        /// <param name="client">client acquired from listener</param>
        public ServerClient(TcpClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// A method for reading thread
        /// </summary>
        public void ReadMessage()
        {
            try
            {
                NetworkStream clientStream = client.GetStream();
                StreamReader reader = new StreamReader(clientStream);

                while (true)
                {
                    string rcv;
                    rcv = reader.ReadLine();

                    if (rcv != null)
                    {
                        lock(_packageReadQueue)
                        {
                            _packageReadQueue.Enqueue(new Package(rcv));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Client connecting failed in Read: " + e.Message);
            }
        }


        /// <summary>
        /// A method for sending thread
        /// </summary>
        public void SendMessage()
        {
            try
            {
                NetworkStream clientStream = client.GetStream();
                StreamWriter writer = new StreamWriter(clientStream);

                while (true)
                {
                    lock (_packageSendQueue)
                    {
                        if (_packageSendQueue.Count > 0)
                        {
                            writer.WriteLine(_packageSendQueue.Dequeue().S);
                            writer.Flush();
                        }
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Client connecting failed in Send: " + e.Message);
            }
        }
    }
}
