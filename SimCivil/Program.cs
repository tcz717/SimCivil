using System;
using System.Collections.Generic;
using SimCivil.Net;

namespace SimCivil
{
    class Program
    {
        static void Main(string[] args)
        {
            int port;
            do
                Console.WriteLine("Enter port to start server:");
            while (!int.TryParse(Console.ReadLine(), out port));

            // Define send and read package queue
            Queue<Package> packageSendQueue = new Queue<Package>();
            Queue<Package> packageReadQueue = new Queue<Package>();

            // Enqueue some send tests
            for (int i = 0; i < 20; i++)
            {
                packageSendQueue.Enqueue(new Package("###test string###---" + i));
            }

            // Initialize server listener and start
            SeverListener server = new SeverListener(port, packageSendQueue, packageReadQueue);
            server.Start();

            #region send test
            while (true)
            {
                Package pk = new Package(Console.ReadLine());
                lock (packageSendQueue)
                {
                    packageSendQueue.Enqueue(pk);
                }
            }
            #endregion

            #region read test
            //while (true)
            //{
            //    lock (packageReadQueue)
            //    {
            //        if (packageReadQueue.Count > 0)
            //        {
            //            Console.WriteLine(packageReadQueue.Dequeue().S);
            //        }
            //    }
            //}
            #endregion

            Console.ReadLine();
        }
    }
}