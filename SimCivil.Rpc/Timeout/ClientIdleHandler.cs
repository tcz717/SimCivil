using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Rpc.Timeout
{
    internal class ClientIdleHandler : ChannelDuplexHandler
    {
        private Action _heartbeatNeeded;

        public ClientIdleHandler(Action heartbeatNeeded)
        {
            _heartbeatNeeded = heartbeatNeeded;
        }

        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            if (evt is IdleStateEvent idleState)
            {
                if (idleState.State == IdleState.WriterIdle)
                {
                    _heartbeatNeeded();
                }
            }

            base.UserEventTriggered(context, evt);
        }
    }
}
