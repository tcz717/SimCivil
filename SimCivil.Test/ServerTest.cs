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

            Assert.Equal(head.packageID, id);
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
            //EndPoint[] endPoints = new EndPoint[10];

            ServerListener serverListener = new ServerListener(DefaultPort);
            serverListener.Start();
            //serverListener.NewConnectionEvent += delegate (EndPoint end)
            //{
            //    endPoints[endPoints.Length] = end;
            //};
            Thread.Sleep(1000);

            Client.Start(DefaultPort);
            Thread.Sleep(2000);
            
            var dataToSend = new Dictionary<string, object>() { { "foo", (long)1 }, { "bar", (long)2 } };
            var head = new Head(1, PacketType.Ping);
            var clients = serverListener.Clients;
            var enu = clients.GetEnumerator();
            enu.MoveNext();
            var client = enu.Current.Value;
            serverListener.PacketSendQueue.Enqueue(new Ping(dataToSend, head, client));

            Thread.Sleep(1000);
            var dataFromClient = Client.receivedPackets.Dequeue().Data;
            Client.Stop();

            Assert.Equal(dataFromClient["foo"], dataToSend["foo"]);
            Assert.Equal(dataFromClient["bar"], dataToSend["bar"]);
        }

        [Fact]
        public void ServerReadTest()
        {

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
