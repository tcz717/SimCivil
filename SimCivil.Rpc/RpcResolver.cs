// Copyright (c) 2017 TPDT
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
// SimCivil - SimCivil.Rpc - RpcResolver.cs
// Create Date: 2017/12/31
// Update Date: 2018/01/01

using System;
using System.Collections.Generic;

using Autofac;

using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace SimCivil.Rpc
{
    public class RpcResolver : SimpleChannelInboundHandler<RpcRequest>
    {
        private ILifetimeScope _scope;
        public RpcServer RpcServer { get; }

        public RpcResolver(RpcServer rpcServer)
        {
            RpcServer = rpcServer;
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            _scope = RpcServer.Container.BeginLifetimeScope();
            base.ChannelActive(context);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            _scope.Dispose();
            base.ChannelInactive(context);
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, RpcRequest msg)
        {
            if (!RpcServer.Services.ContainsKey(msg.ServiceName))
            {
                throw new ArgumentException(nameof(msg.ServiceName));
            }

            RpcServer.OnRemoteCalling(msg);

            var type = RpcServer.Services[msg.ServiceName];
            var service = _scope.Resolve(type);
            var returnValue = type.GetMethod(msg.MethodName).Invoke(service, msg.Arguments);
            ctx.Channel.WriteAndFlushAsync(new RpcResponse(msg, returnValue));
        }
    }
}