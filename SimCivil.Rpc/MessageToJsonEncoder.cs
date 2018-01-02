using System.Text;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Newtonsoft.Json;

namespace SimCivil.Rpc
{
    public class MessageToJsonEncoder<T> : MessageToByteEncoder<T>
    {
        public MessageToJsonEncoder()
        {
        }

        protected override void Encode(IChannelHandlerContext context, T message, IByteBuffer output)
        {
            output.WriteBytes(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message, Formatting.None)));
        }
    }
}