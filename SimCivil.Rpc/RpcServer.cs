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
// Update Date: 2018/12/11

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SimCivil.Rpc.Callback;
using SimCivil.Rpc.Session;
using SimCivil.Rpc.Timeout;
using static System.Diagnostics.Debug;

namespace SimCivil.Rpc
{
    public class RpcServer
    {
        private readonly IEventLoopGroup _workerGroup = new MultithreadEventLoopGroup();
        public Dictionary<string, Type> Services { get; } = new Dictionary<string, Type>();

        public IPEndPoint EndPoint { get; set; }

        public IChannel ServerChannel { get; private set; }

        public IContainer Container { get; }

        public ITimeoutDaemon TimeoutDaemon { get; }

        public bool SupportSession { get; }
        public RpcSessionManager Sessions { get; } = new RpcSessionManager();
        public bool Debug { get; set; } = false;

        protected ILogger log;
        protected ILoggerFactory logFac;

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
                Assert(serviceType.FullName != null, "serviceType.FullName != null");
                Services.Add(serviceType.FullName, serviceType);
            }

            log = GetLogger<RpcServer>();
            
            // Create Timeout daemon
            TimeoutDaemon = new SimpleTimeoutDaemon(this, GetLogger<SimpleTimeoutDaemon>(), 5000);
            TimeoutDaemon.ClientTimeout += (sender, args) => CloseChannel(args.Channel);

            log.LogInformation("RpcServer initialized");
        }

        private void CloseChannel(IChannel channel)
        {
            if (channel.Open)
            {
                channel.CloseAsync().Wait();
                log.LogInformation($"Channel closed: {channel}");
            }
            else
            {
                log.LogWarning($"Channel already closed while closing: {channel}");
            }
        }

        /// <summary>
        /// Get logger factory and create logger with specific type
        /// </summary>
        /// <typeparam name="T">Category</typeparam>
        /// <returns>Logger</returns>
        protected ILogger<T> GetLogger<T>()
        {
            ILogger<T> logger;
            if (logFac == null)
            {
                if (Container.IsRegistered<ILoggerFactory>())
                {
                    logFac = Container.Resolve<ILoggerFactory>();
                }
                else
                {
                    logFac = new LoggerFactory();
                }
            }
            logger = logFac.CreateLogger<T>();
            return logger;
        }

        public event EventHandler<EventArgs<RpcRequest>> RemoteCalling;

        public RpcServer Bind(int port)
        {
            EndPoint = new IPEndPoint(IPAddress.Any, port);
            log.LogInformation($"Server bind to port: {port}");

            return this;
        }

        public RpcServer Bind(string ip, int port)
        {
            EndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            log.LogInformation($"Server bind to port: {port}");

            return this;
        }

        public RpcServer Expose<T>()
        {
            Type type = typeof(T);
            if (!Container.IsRegistered(type))
            {
                throw new ArgumentNullException(nameof(T));
            }

            Assert(type.FullName != null, "type.FullName != null");
            Services[type.FullName] = type;

            return this;
        }

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
                TimeoutDaemon.Start();
            }
            catch (Exception ex)
            {
                log.LogCritical("Server fatal error while running", ex.Message);
                Task.WaitAll(bossGroup.ShutdownGracefullyAsync(), _workerGroup.ShutdownGracefullyAsync());
                TimeoutDaemon.Stop();
                throw;
            }
        }

        protected virtual void ChildChannelInit(IChannel channel)
        {
            channel.Pipeline.AddLast(new HttpRequestHandler())
                .AddLast(new LengthFieldPrepender(2))
                .AddLast(new Log4NetHandler())
                .AddLast(new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2))
                .AddLast(new JsonToMessageDecoder())
                .AddLast(new MessageToJsonEncoder<RpcResponse>())
                .AddLast(new MessageToJsonEncoder<RpcCallback>())
                .AddLast(new RpcResolver(this));
        }

        public void Stop()
        {
            TimeoutDaemon.Stop();
            _workerGroup.ShutdownGracefullyAsync().Wait();
            ServerChannel?.CloseAsync().Wait();
        }

        public virtual void OnRemoteCalling(RpcRequest e)
        {
            RemoteCalling?.Invoke(this, new EventArgs<RpcRequest>(e));
        }
    }
}