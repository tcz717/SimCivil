using DotNetty.Transport.Channels;

namespace SimCivil.Rpc
{
    public class RpcSession : ChannelHandlerAdapter
    {
        public RpcSession()
        {
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);
        }
    }
}