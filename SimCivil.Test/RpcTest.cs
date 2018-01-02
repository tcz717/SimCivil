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
// Create Date: 2018/01/01
// Update Date: 2018/01/01

using System;
using System.Collections.Generic;
using System.Text;

using Autofac;

using FakeItEasy;

using Newtonsoft.Json;

using SimCivil.Rpc;

using Xunit;
using Xunit.Abstractions;

namespace SimCivil.Test
{
    public interface ITestService
    {
        string HelloWorld(string name);
    }

    public class TestService : ITestService
    {
        public string HelloWorld(string name)
        {
            return $"Hello {name}!";
        }
    }

    public class RpcTest : IDisposable
    {
        public RpcTest(ITestOutputHelper output)
        {
            Output = output;
            JsonToMessageDecoder<RpcResponse>.TestHook = s => output.WriteLine($"Get {nameof(RpcResponse)}: {s}");
            JsonToMessageDecoder<RpcRequest>.TestHook = s => output.WriteLine($"Get {nameof(RpcRequest)}: {s}");

            var builder = new ContainerBuilder();
            builder.RegisterType<TestService>().As<ITestService>().InstancePerLifetimeScope();

            Server = new RpcServer(builder.Build());
            Server.Bind(9999)
                .Expose<ITestService>();
            Server.Run().Wait();
        }

        public void Dispose()
        {
            Server?.Stop();
        }

        public RpcServer Server { get; private set; }

        public ITestOutputHelper Output { get; }

        [Fact]
        public void ProxyTest()
        {
            RpcClient client = new RpcClient();
            client.Bind(9999).Connect().Wait();

            string name = "test";

            var service = client.Import<ITestService>();
            Assert.RaisesAny<EventArgs<RpcRequest>>(
                e => Server.RemoteCalling += e,
                e => Server.RemoteCalling -= e,
                () => { Assert.Equal(service.HelloWorld(name), $"Hello {name}!"); });
        }
    }
}