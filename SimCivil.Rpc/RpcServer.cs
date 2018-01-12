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
// SimCivil - SimCivil.Rpc - RpcServer.cs
// Create Date: 2018/01/02
// Update Date: 2018/01/03

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using DotNetty.Codecs;
using DotNetty.Common.Concurrency;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

using SimCivil.Rpc.Session;

namespace SimCivil.Rpc
{
    public class RpcServer
    {
        public Dictionary<string, Type> Services { get; } = new Dictionary<string, Type>();

        public IPEndPoint EndPoint { get; set; }

        public IChannel ServerChannel { get; private set; }

        public IContainer Container { get; }

        public bool SupportSession { get; }
        public RpcSessionManager Sessions { get; } = new RpcSessionManager();
        public bool Debug { get; set; } = false;

        internal RpcServer()
        {
            // TODO Set InternalLoggerFactory.DefaultFactory
        }

        public RpcServer(IContainer container)
            : this()
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));
            SupportSession = container.IsRegistered<IRpcSession>();
            foreach (Type serviceType in container.GetRpcServiceTypes())
            {
                Services.Add(serviceType.FullName, serviceType);
            }
        }

        public event EventHandler<EventArgs<RpcRequest>> RemoteCalling;

        public RpcServer Bind(int port)
        {
            EndPoint = new IPEndPoint(IPAddress.Any, port);

            return this;
        }

        public RpcServer Bind(string ip, int port)
        {
            EndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            return this;
        }

        public RpcServer Expose<T>()
        {
            Type type = typeof(T);
            if (!Container.IsRegistered(type))
            {
                throw new ArgumentNullException(nameof(T));
            }

            Services[type.FullName] = type;

            return this;
        }

        private readonly IEventLoopGroup _workerGroup = new MultithreadEventLoopGroup();

        public async Task Run()
        {
            IEventLoopGroup bossGroup = new MultithreadEventLoopGroup();
            try
            {
                ServerBootstrap bootstrap = new ServerBootstrap();
                bootstrap
                    .Group(bossGroup, _workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .Option(ChannelOption.SoBacklog, 100)
                    .ChildOption(ChannelOption.TcpNodelay, true)
                    .ChildOption(ChannelOption.SoKeepalive, true)
                    .ChildHandler(
                        new ActionChannelInitializer<IChannel>(
                            ChildChannelInit));

                IChannel serverChannel = await bootstrap.BindAsync(EndPoint);
                ServerChannel = serverChannel;
            }
            catch
            {
                Task.WaitAll(bossGroup.ShutdownGracefullyAsync(), _workerGroup.ShutdownGracefullyAsync());

                throw;
            }
        }

        protected virtual void ChildChannelInit(IChannel channel)
        {
            channel.Pipeline.AddLast(new LengthFieldPrepender(2))
                .AddLast(new Log4NetHandler())
                .AddLast(new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2))
                .AddLast(new JsonToMessageDecoder<RpcRequest>())
                .AddLast(new MessageToJsonEncoder<RpcResponse>())
                .AddLast(new RpcResolver(this));
        }

        public void Stop()
        {
            _workerGroup.ShutdownGracefullyAsync().Wait();
            ServerChannel?.CloseAsync().Wait();
        }

        public virtual void OnRemoteCalling(RpcRequest e)
        {
            RemoteCalling?.Invoke(this, new EventArgs<RpcRequest>(e));
        }
    }
}