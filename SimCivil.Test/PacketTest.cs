using SimCivil.Net;
using SimCivil.Net.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace SimCivil.Test
{
    public class PacketTest
    {
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
        public void ToBytes()
        {
            //TODO
            throw new NotImplementedException();
        }

        [Theory]
        [InlineData(1, PacketType.Ping, 0)]
        public void PacketCreate(int id, PacketType type, int size)
        {
            var pkt = PacketFactory.Create(
                new ServerClient(null, null),
                new Head(id, type, size),
                new byte[0]
                );
            Assert.IsType<Ping>(pkt);
        }
    }
}
