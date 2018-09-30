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
// SimCivil - SimCivil.IntegrationTest - MovementTest.cs
// Create Date: 2018/09/29
// Update Date: 2018/09/30

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SimCivil.Contract;
using SimCivil.Contract.Model;
using SimCivil.Rpc;

namespace SimCivil.IntegrationTest.Testcase
{
    public class MovementTest : IIntegrationTest, IDisposable
    {
        private RpcClient _rpcClient;
        public ILogger<MovementTest> Logger { get; }

        public MovementTest(ILogger<MovementTest> logger)
        {
            Logger = logger;
        }

        /// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
        public void Dispose()
        {
            _rpcClient?.Dispose();
        }

        public async Task Test()
        {
            _rpcClient = new RpcClient(
                new IPEndPoint(
                    Dns.GetHostAddresses(Dns.GetHostName()).First(ip => ip.AddressFamily == AddressFamily.InterNetwork),
                    20170));
            await _rpcClient.ConnectAsync();

            var auth = _rpcClient.Import<IAuth>();
            await auth.LogInAsync("admin", "");
            var rm = _rpcClient.Import<IRoleManager>();
            await rm.CreateRole(new CreateRoleOption() {Gender = Gender.Male, Name = "test", Race = Race.Human});
            await rm.UseRole((await rm.GetRoleList()).First().Id);

            var sync = _rpcClient.Import<IViewSynchronizer>();
            sync.RegisterViewSync(
                vc => { Logger.LogInformation(vc.ToString()); });

            var controller = _rpcClient.Import<IPlayerController>();

            await controller.Move((1, 0), 1);
            await Task.Delay(1000);
            await controller.Stop();

            Logger.LogInformation($"{nameof(MovementTest)} end");
        }
    }
}