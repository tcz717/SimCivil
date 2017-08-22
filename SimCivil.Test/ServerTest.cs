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

    public class ServerTest : IDisposable
    {
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
            Packet packetSend = new Ping(dataSend);

            // Convert Packet to bytes
            byte[] bufferSend = packetSend.ToBytes(1);

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

        private byte[] ToData(int id, PacketType type, int size)
        {
            MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(id);
                writer.Write((int)type);
                writer.Write(size);
            }
            var data = stream.ToArray();
            output.WriteLine($"Raw data is: {Convert.ToBase64String(data)}");
            return stream.ToArray();
        }

        public void Dispose()
        {
        }

        public ServerTest(ITestOutputHelper outputHelper)
        {
            output = outputHelper;
        }
    }
}
