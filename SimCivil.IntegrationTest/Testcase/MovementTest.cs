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
// Update Date: 2018/10/17

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Orleans;

using SimCivil.Contract;
using SimCivil.Contract.Model;
using SimCivil.Orleans.Interfaces;
using SimCivil.Rpc;

namespace SimCivil.IntegrationTest.Testcase
{
    public class MovementTest : IIntegrationTest, IDisposable
    {
        private static int _id;
        protected RpcClient Client { get; }
        public ILogger<MovementTest> Logger { get; }
        public IClusterClient Cluster { get; }

        public string RoleName { get; set; }

        public MovementTest(ILogger<MovementTest> logger, IClusterClient cluster)
        {
            RoleName = nameof(MovementTest) + Interlocked.Increment(ref _id);
            Logger = logger;
            Cluster = cluster;
            Client = new RpcClient(
                new IPEndPoint(
                    Dns.GetHostAddresses(Dns.GetHostName()).First(ip => ip.AddressFamily == AddressFamily.InterNetwork),
                    20170));
        }

        /// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
        public void Dispose()
        {
            if (IsRunning)
                Stop().Wait();
            Client?.Dispose();
        }

        public bool IsRunning { get; private set; }

        public async Task Test()
        {
            IsRunning = true;
            await Client.ConnectAsync();
            await RegisterAndLogin();

            var rm = Client.Import<IRoleManager>();
            await rm.CreateRole(new CreateRoleOption {Gender = Gender.Male, Name = RoleName, Race = Race.Human});
            await rm.UseRole((await rm.GetRoleList()).First().Id);

            var sync = Client.Import<IViewSynchronizer>();
            (float x, float y) pos = (0, 0);
            float speed = 0;
            sync.RegisterViewSync(
                vc =>
                {
                    pos = vc.Position;
                    speed = vc.Speed;
                    if (vc.EntityChange?.Any() ?? false)
                        Logger.LogInformation(vc.EntityChange.First().ToString());
                    else
                        Logger.LogDebug(vc.ToString());
                });

            var controller = Client.Import<IPlayerController>();

            await Task.Delay(500);
            for (int i = 0; i < 100; i++)
            {
                await controller.MoveTo((pos.x + speed * 0.05f, pos.y), DateTime.UtcNow);

                await Task.Delay(50);
            }

            Logger.LogInformation($"{RoleName} test end");
        }

        public async Task Stop()
        {
            var sync = Client.Import<IViewSynchronizer>();
            sync.DeregisterViewSync();
            var rm = Client.Import<IRoleManager>();
            await rm.ReleaseRole();
            Client.Disconnect();
            IsRunning = false;
        }

        public Guid GetEntityId()
        {
            var rm = Client.Import<IRoleManager>();

            return Task.Factory.StartNew(() => rm.GetRoleList().Result).Result.First().Id;
        }

        protected async Task RegisterAndLogin(string name = null, string password = "")
        {
            if (name == null)
            {
                name = RoleName;
            }

            await Cluster.GetGrain<IAccount>(name).Register(password);

            var auth = Client.Import<IAuth>();
            await auth.LogInAsync(name, password);

            Logger.LogInformation($"Role \"{RoleName}\" created and login");
        }

        /// <summary>返回表示当前对象的字符串。</summary>
        /// <returns>表示当前对象的字符串。</returns>
        public override string ToString()
        {
            return RoleName;
        }
    }
}