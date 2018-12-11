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
// SimCivil - SimCivil.Test - RpcTest.cs
// Create Date: 2018/01/02
// Update Date: 2018/01/31

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Autofac;

using SimCivil.Rpc;

using Xunit;
using Xunit.Abstractions;

namespace SimCivil.Test
{
    public class ServerFixture : IDisposable
    {
        public RpcServer Server { get; set; }

        public ServerFixture()
        {
            var builder = new ContainerBuilder();
            builder.UseRpcSession();
            builder.RegisterRpcProvider<TestServiceA, ITestServiceA>().InstancePerChannel();
            builder.RegisterRpcProvider<TestServiceB, ITestServiceB>().SingleInstance();

            Server = new RpcServer(builder.Build()) {Debug = true};
            Server.Bind(9999);
            Server.Run().Wait();
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Server?.Stop();
        }
    }

    public class RpcTest : IDisposable, IClassFixture<ServerFixture>
    {
        public RpcTest(ITestOutputHelper output, ServerFixture server)
        {
            Output = output;
            JsonToMessageDecoder<RpcResponse>.TestHook = s => output.WriteLine($"Get {nameof(RpcResponse)}: {s}");
            JsonToMessageDecoder<RpcRequest>.TestHook = s => output.WriteLine($"Get {nameof(RpcRequest)}: {s}");
            JsonToMessageDecoder.TestHook = s => output.WriteLine($"Get: {s}");

            Server = server.Server;
        }

        public RpcServer Server { get; set; }

        public void Dispose() { }

        public ITestOutputHelper Output { get; }

        [Fact]
        public void AsyncSessionTest()
        {
            using (RpcClient client = new RpcClient())
            {
                client.Bind(9999).ConnectAsync().Wait();

                var service = client.Import<ITestServiceB>();
                Assert.Equal(service.CheckAsync().Result, client.Channel.LocalAddress);
            }
        }

        [Fact]
        public void AsyncTest()
        {
            using (RpcClient client = new RpcClient())
            {
                client.Bind(9999).ConnectAsync().Wait();

                var service = client.Import<ITestServiceB>();

                string msg = "56456545fsdgfddsfsfs";
                service.EchoAsync(msg)
                    .ContinueWith(
                        resp =>
                            Assert.Equal(resp.Result, msg))
                    .Wait();
            }
        }

        [Trait("Category", "Performance")]
        [Fact]
        public void AsyncParallelTest()
        {
            const int testNum = 1000;
            Task[] tasks = new Task[testNum];
            Stopwatch[] stopwatches = new Stopwatch[testNum];
            using (RpcClient client = new RpcClient())
            {
                client.Bind(9999).ConnectAsync().Wait();

                var serviceB = client.Import<ITestServiceB>();
                for (var i = 0; i < tasks.Length; i++)
                {
                    int _i = i;
                    stopwatches[i] = Stopwatch.StartNew();
                    tasks[i] = serviceB.EchoAsync(i.ToString())
                        .ContinueWith(
                            t =>
                            {
                                stopwatches[_i].Stop();
                                Assert.Equal(_i.ToString(), t.Result);
                            });
                }

                Task.WaitAll(tasks);
            }

            Output.WriteLine(
                $"Average [EchoAsync] time cost is {stopwatches.Select(s => s.ElapsedMilliseconds).Average()} ms.");
        }

        [Fact]
        public void CallbackProxyTest()
        {
            using (RpcClient client = new RpcClient())
            {
                AutoResetEvent resetEvent = new AutoResetEvent(false);
                client.Bind(9999).ConnectAsync().Wait();

                var service = client.Import<ITestServiceA>();
                
                service.Echo("123", s => resetEvent.Set());

                Assert.True(resetEvent.WaitOne(1000));
            }
        }

        [Fact]
        public void FilterTest()
        {
            using (RpcClient client = new RpcClient())
            {
                client.Bind(9999).ConnectAsync().Wait();

                var service = client.Import<ITestServiceB>();

                Assert.Throws<RemotingException>(() => service.DeniedAction());
            }
        }

        [Fact]
        public void ValueTupleTest()
        {
            using (RpcClient client = new RpcClient())
            {
                client.Bind(9999).ConnectAsync().Wait();

                var service = client.Import<ITestServiceA>();

                var dump = (1.2, 3.4);
                Assert.Equal(dump,service.TupleEcho(dump));
            }
        }

        [Fact]
        public void AsyncValueTupleTest()
        {
            using (RpcClient client = new RpcClient())
            {
                client.Bind(9999).ConnectAsync().Wait();

                var service = client.Import<ITestServiceB>();

                var dump = (1.2, 3.4);
                Assert.Equal(dump, service.TupleEchoAsync(dump).Result);
            }
        }

        [Fact]
        public void PerChannelScopeTest()
        {
            using (RpcClient client1 = new RpcClient())
            {
                using (RpcClient client2 = new RpcClient())
                {
                    client1.Bind(9999).ConnectAsync().Wait();
                    client2.Bind(9999).ConnectAsync().Wait();

                    var service1 = client1.Import<ITestServiceA>();
                    var service2 = client2.Import<ITestServiceA>();

                    Assert.NotEqual(service1.HelloWorld("1"), service2.HelloWorld("2"));
                }
            }
        }

        [Fact]
        public void ProxyTest()
        {
            using (RpcClient client = new RpcClient())
            {
                client.Bind(9999).ConnectAsync().Wait();

                var name = "test";

                var service = client.Import<ITestServiceA>();

                Assert.RaisesAny<EventArgs<RpcRequest>>(
                    e => Server.RemoteCalling += e,
                    e => Server.RemoteCalling -= e,
                    () =>
                    {
                        Assert.Equal(service.HelloWorld(name), $"Hello {name}!");
                        Assert.Equal(service.GetName(), name);
                    });

                var serviceB = client.Import<ITestServiceB>();
            }
        }

        [Trait("Category", "Performance")]
        [Fact]
        public void SessionRequiredParallelTest()
        {
            const int testNum = 100;
            long sum = 0;
            Parallel.For<long>(
                0,
                testNum,
                () => 0,
                (i, state, total) =>
                {
                    Stopwatch stopwatch = new Stopwatch();
                    using (RpcClient client = new RpcClient())
                    {
                        client.Bind(9999).ConnectAsync().Wait();

                        var serviceA = client.Import<ITestServiceA>();
                        var serviceB = client.Import<ITestServiceB>();

                        serviceB.SetSession("test", i.ToString());

                        stopwatch.Start();
                        string expected = serviceA.GetSession("test");
                        stopwatch.Stop();
                        Assert.Equal(expected, i.ToString());
                    }

                    return stopwatch.ElapsedMilliseconds + total;
                },
                total => Interlocked.Add(ref sum, total));
            Output.WriteLine($"Average [GetSession] time cost is {sum / (double) testNum} ms.");
        }

        [Fact]
        public void SessionTest()
        {
            using (RpcClient client1 = new RpcClient())
            {
                using (RpcClient client2 = new RpcClient())
                {
                    client1.Bind(9999).ConnectAsync().Wait();
                    client2.Bind(9999).ConnectAsync().Wait();

                    var serviceA1 = client1.Import<ITestServiceA>();
                    var serviceA2 = client2.Import<ITestServiceA>();
                    var serviceB1 = client1.Import<ITestServiceB>();
                    var serviceB2 = client2.Import<ITestServiceB>();

                    serviceB1.SetSession("test", "1");
                    serviceB2.SetSession("test", "2");

                    Assert.NotEqual(serviceA1.GetSession("test"), serviceA2.GetSession("test"));
                    Assert.Equal("1", serviceA1.GetSession("test"));
                    Assert.Equal("2", serviceA2.GetSession("test"));
                }
            }
        }

        [Fact]
        public void ThrowErrorTest()
        {
            using (RpcClient client = new RpcClient())
            {
                client.Bind(9999).ConnectAsync().Wait();

                var service = client.Import<ITestServiceA>();

                Assert.ThrowsAny<RemotingException>(() => service.NotImplementedFuc(1));
            }
        }
    }
}