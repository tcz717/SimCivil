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
using System.Linq;

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
            // Create a Packet and enqueue it for sending
            var head = new Head(1, PacketType.Ping);
            var dataToSend = new Dictionary<string, object>() { { "foo", 1L }, { "bar", 2L } };
            // Start server and subscribe connection event
            ServerListener serverListener = new ServerListener(DefaultPort);
            Client virtualClient = new Client();
            serverListener.NewConnectionEvent += (sender, e) =>
            {
                e.SendPacket(new Ping(dataToSend, head, e));
            };
            serverListener.Start();
            Thread.Sleep(500); // Keep it long enough to start server before starting client

            // Start client
            virtualClient.Start(DefaultPort);

            // Wait for sending and get data from client
            int retry = 10;
            while (retry > 0)
            {
                Thread.Sleep(100);
                if(virtualClient.receivedPackets.Any())
                {
                    var dataFromClient = virtualClient.receivedPackets.Dequeue().Data;

                    Assert.Equal(dataFromClient["foo"], dataToSend["foo"]);
                    Assert.Equal(dataFromClient["bar"], dataToSend["bar"]);
                    break;
                }
            }
            Assert.True(retry > 0);

            // Test removing client function
            virtualClient.Stop();
            serverListener.StopAndRemoveAllClient();
        }

        [Fact]
        public void ServerReadTest()
        {
            Dictionary<string, object> dataToSend = new Dictionary<string, object>();
            Semaphore readSem = new Semaphore(0, 10);

            // Start server and subscribe connection event
            ServerListener serverListener = new ServerListener(DefaultPort+1);
            Client virtualClient = new Client();
            serverListener.NewConnectionEvent += (sender,e) =>
            {
                readSem.Release();
                e.OnPacketReceived += (sender0, e0) =>
                  {
                      readSem.Release();
                  };
            };
            serverListener.Start();

            // Start client
            virtualClient.Start(DefaultPort+1);

            // Send Packet from virtual client
            var head = new Head(2, PacketType.Ping);
            dataToSend = new Dictionary<string, object>() { { "foo", 3.1415 }, { "bar", 0.12345 } };
            virtualClient.PacketsForSend.Enqueue(new Ping(dataToSend, head, null));
            
            Assert.True(readSem.WaitOne());
            Assert.True(readSem.WaitOne(5000));

            // Test removing client function
            virtualClient.Stop();
            serverListener.StopAndRemoveAllClient();
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
