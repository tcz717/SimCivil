﻿// Copyright (c) 2017 TPDT
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
// Update Date: 2018/01/02

using System;
using System.Text;

using Autofac;

using SimCivil.Rpc;
using SimCivil.Rpc.Session;

using Xunit;
using Xunit.Abstractions;

namespace SimCivil.Test
{
    public interface ITestServiceA
    {
        string GetName();
        string HelloWorld(string name);
        int NotImplementedFuc(int i);
        string GetSession(string key);
    }

    public interface ITestServiceB
    {
        void SetSession(string key, string value);
    }

    class TestServiceB : ITestServiceB
    {
        public IRpcSession Session { get; }

        public TestServiceB(IRpcSession session)
        {
            Session = session;
        }
        public void SetSession(string key, string value)
        {
            Session[key] = value;
        }
    }

    public class TestServiceA : ITestServiceA
    {
        public IRpcSession Session { get; }
        public string Name { get; set; }

        public TestServiceA(IRpcSession session)
        {
            Session = session;
        }

        public string GetName()
        {
            return Name;
        }

        public string HelloWorld(string name)
        {
            Name = name;

            return $"Hello {name}!";
        }

        public int NotImplementedFuc(int i)
        {
            throw new NotImplementedException();
        }

        public string GetSession(string key)
        {
            return Session[key].ToString();
        }
    }

    public class RpcTest : IDisposable
    {
        public RpcTest(ITestOutputHelper output)
        {
            Output = output;
            JsonToMessageDecoder<RpcResponse>.TestHook = s => Output.WriteLine($"Get {nameof(RpcResponse)}: {s}");
            JsonToMessageDecoder<RpcRequest>.TestHook = s => Output.WriteLine($"Get {nameof(RpcRequest)}: {s}");

            var builder = new ContainerBuilder();
            builder.UseRpcSession();
            builder.RegisterRpcProvider<TestServiceA, ITestServiceA>().InstancePerChannel();
            builder.RegisterRpcProvider<TestServiceB, ITestServiceB>().InstancePerChannel();

            Server = new RpcServer(builder.Build());
            Server.Bind(9999);
            Server.Run().Wait();
        }

        public void Dispose()
        {
            Server?.Stop();
        }

        public RpcServer Server { get; private set; }

        public ITestOutputHelper Output { get; }

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
                    Assert.Equal(serviceA1.GetSession("test"), "1");
                    Assert.Equal(serviceA2.GetSession("test"), "2");
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