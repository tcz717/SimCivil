using SimCivil.Net;
using SimCivil.Net.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace SimCivil.Test
{

    public class PacketTest
    {
        // How to use this? https://xunit.github.io/docs/capturing-output.html#output-in-tests
        private readonly ITestOutputHelper output;
        
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
            Dictionary<String, object> dataSend = new Dictionary<String, object> { { "int", 1 }, { "float", 1.22f } };
            Head headSend = new Head(1, PacketType.Ping);
            Packet packetSend = new Ping(dataSend, headSend);

            // Convert Packet to bytes
            byte[] bufferSend = packetSend.ToBytes();

            #region transmit as stream
            //// Transmit data
            //MemoryStream stream = new MemoryStream();
            //stream.Write(bufferSend, 0, bufferSend.Length);

            //// Receive Head bytes from stream
            //byte[] bufferRead = new byte[Packet.MaxSize];
            //int lengthOfHead = stream.Read(bufferRead, 0, Head.HeadLength);

            //// Build Head
            //Head head = Head.FromBytes(bufferRead);

            //// Build Packet
            //int lengthOfBody = stream.Read(bufferRead, 0, head.length);
            //Packet packetRcv = PacketFactory.Create(null , head, bufferRead);
            #endregion

            #region directly read
            // Receive Head bytes
            byte[] bufferRead = bufferSend;
            byte[] bufferHead = new byte[12];
            Array.Copy(bufferRead, bufferHead, 12);

            // Build Head
            Head head = Head.FromBytes(bufferHead);

            // Build Packet
            byte[] bufferBody = new byte[4096];
            Array.Copy(bufferRead, 12, bufferBody, 0, head.length);
            Packet packetRcv = PacketFactory.Create(null, head, bufferBody);
            #endregion

            // Compare result
            Assert.Equal<PacketType>(packetRcv.Head.type, packetSend.Head.type);
            Assert.Equal(packetRcv.Data["int"], packetSend.Data["int"]);
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
    }
}
