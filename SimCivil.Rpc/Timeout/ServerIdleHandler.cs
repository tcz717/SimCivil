using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;

namespace SimCivil.Rpc.Timeout
{
    internal class ServerIdleHandler : ChannelDuplexHandler
    {
        protected ILogger Logger { get; set; }

        public ServerIdleHandler(ILogger logger)
        {
            Logger = logger;
        }

        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            if (evt is IdleStateEvent idleState)
            {
                if (idleState.State == IdleState.ReaderIdle)
                {
                    context.Channel.CloseAsync().Wait();
                    Logger.LogInformation($"Channel closed due to timeout: {context.Channel.RemoteAddress}");
                }
            }

            base.UserEventTriggered(context, evt);
        }
    }
}
