using SimCivil.Net;
using SimCivil.Net.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Net;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft.Json;
using SimCivil.Test.VirtualClient;
using static SimCivil.Config;

namespace SimCivil.Test
{

    public class ServerTest
    {
        // How to use this? https://xunit.github.io/docs/capturing-output.html#output-in-tests
        private readonly ITestOutputHelper output;

        [Theory]
        [InlineData(1, PacketType.Ping, 3)]
        public void HeadToBytes(int id, PacketType type, int size)
        {
            var head = new Head(id, type, size);

            var data = ToData(id, type, size);

            Assert.Equal(data, head.ToBytes());
            Assert.Equal(12, data.Length);
        }

        [Theory]
        [InlineData(1, PacketType.Ping, 3)]
        public void HeadFrombytes(int id, PacketType type, int size)
        {
            var data = ToData(id, type, size);
            var head = Head.FromBytes(data);

            Assert.Equal(head.packetID, id);
            Assert.Equal(head.type, type);
            Assert.Equal(head.length, size);
        }

        [Fact]
        public void PacketToAndFromBytes()
        {
            // Init a Packet
            int a = 1; long a1 = 100; float b = 3.141592f; double c = 3.1415926535;
            Dictionary<String, object> dataSend = 
                new Dictionary<String, object> { { "int", a }, { "long", a1 }, { "float", b }, { "double", c } };
            Head headSend = new Head(1, PacketType.Ping);
            Packet packetSend = new Ping(dataSend, headSend);

            // Convert Packet to bytes
            byte[] bufferSend = packetSend.ToBytes();

            #region transmit as stream
            // Transmit data
            MemoryStream stream = new MemoryStream();
            stream.Write(bufferSend, 0, bufferSend.Length);

            // Receive Head bytes from stream
            byte[] bufferRead = new byte[Packet.MaxSize];
            stream.Position = 0;
            int lengthOfHead = stream.Read(bufferRead, 0, Head.HeadLength);

            // Build Head
            Head head = Head.FromBytes(bufferRead);

            // Build Packet
            int lengthOfBody = stream.Read(bufferRead, 0, head.length);
            Packet packetRcv = PacketFactory.Create(null, head, bufferRead);
            #endregion

            // Compare result
            Assert.Equal<PacketType>(packetRcv.Head.type, packetSend.Head.type);
            Assert.Equal(Convert.ToInt32(packetRcv.Data["int"]), packetSend.Data["int"]);
            Assert.Equal(packetRcv.Data["long"], packetSend.Data["long"]);
            Assert.Equal(Convert.ToSingle(packetRcv.Data["float"]), packetSend.Data["float"]);
            Assert.Equal(packetRcv.Data["double"], packetSend.Data["double"]);
        }

        [Theory]
        [InlineData(1, PacketType.Ping, 100)]
        public void PacketCreate_Shallow(int id, PacketType type, int size)
        {
            Head head = new Head(id, type, size);
            var pkt = PacketFactory.Create(
                null,
                head,
                new byte[head.length]
                );
            Assert.IsType<Ping>(pkt);
        }

        [Fact]
        public void ServerSendTest()
        {
            List<EndPoint> endPoints = new List<EndPoint>();

            // Start server and subscribe connection event
            ServerListener serverListener = new ServerListener(DefaultPort);
            serverListener.NewConnectionEvent += (end) => endPoints.Add(end);
            serverListener.LostConnectionEvent += (end) => endPoints.Remove(end);
            serverListener.Start();
            Thread.Sleep(500); // Keep it long enough to start server before starting client

            // Start client
            Client.Start(DefaultPort);
            Thread.Sleep(500);

            // Create a Packet and enqueue it for sending
            var dataToSend = new Dictionary<string, object>() { { "foo", (long)1 }, { "bar", (long)2 } };
            var head = new Head(1, PacketType.Ping);
            var client = serverListener.Clients[endPoints[0]];
            serverListener.PacketSendQueue.Enqueue(new Ping(dataToSend, head, client));

            // Wait for sending and get data from client
            Thread.Sleep(300);
            var dataFromClient = Client.receivedPackets.Dequeue().Data;

            Assert.Equal(dataFromClient["foo"], dataToSend["foo"]);
            Assert.Equal(dataFromClient["bar"], dataToSend["bar"]);
            
            // Test removing client function
            Client.Stop();
            serverListener.StopAndRemoveClient(serverListener.Clients[endPoints[0]]);
            Thread.Sleep(50);
            Assert.Empty(endPoints);
        }

        [Fact]
        public void ServerReadTest()
        {
            List<EndPoint> endPoints = new List<EndPoint>();
            Dictionary<string, object> dataToSend = new Dictionary<string, object>();
            bool eventIsTriggered = false;

            // Start server and subscribe connection event
            ServerListener serverListener = new ServerListener(DefaultPort);
            serverListener.NewConnectionEvent += (end) => endPoints.Add(end);
            serverListener.LostConnectionEvent += (end) => endPoints.Remove(end);
            Ping.PingEvent += (data) =>
            {
                Assert.Equal(data["foo"], dataToSend["foo"]);
                Assert.Equal(data["bar"], dataToSend["bar"]);
                eventIsTriggered = true;
            };
            serverListener.Start();
            Thread.Sleep(500); // Keep it long enough to start server before starting client

            // Start client
            Client.Start(DefaultPort);
            Thread.Sleep(500);

            // Send Packet from virtual client
            var head = new Head(2, PacketType.Ping);
            dataToSend = new Dictionary<string, object>() { { "foo", 3.1415 }, { "bar", 0.12345 } };
            Client.PacketsForSend.Enqueue(new Ping(dataToSend, head, null));


            // Wait for sending from client and PingEvent from Server
            Thread.Sleep(1000); 
            Assert.Equal(true, eventIsTriggered);

            // Test removing client function
            Client.Stop();
            serverListener.StopAndRemoveClient(serverListener.Clients[endPoints[0]]);
            Thread.Sleep(50);
            Assert.Empty(endPoints);
        }

        private byte[] ToData(int id, PacketType type, int size)
        {
            MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(id);
                writer.Write((int)type);
                writer.Write(size);
            }
            return stream.ToArray();
        }
    }
}
