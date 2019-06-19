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
// SimCivil - SimCivil.IntegrationTest - EntityTestBase.cs
// Create Date: //
// Update Date: 2019/06/18

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Orleans;

using SimCivil.Contract;
using SimCivil.Contract.Model;
using SimCivil.Orleans.Interfaces;
using SimCivil.Rpc;

namespace SimCivil.IntegrationTest.Testcase {
    public abstract class EntityTestBase : IIntegrationTest, IDisposable
    {
        private static int _id;

        protected EntityTestBase(ILogger logger, IClusterClient cluster)
        {
            RoleName = GetType().Name + Interlocked.Increment(ref _id);
            Logger   = logger;
            Cluster  = cluster;
            Client = new RpcClient(
                new IPEndPoint(
                    Dns.GetHostAddresses(Dns.GetHostName()).First(ip => ip.AddressFamily == AddressFamily.InterNetwork),
                    20170));
        }

        protected RpcClient Client { get; }
        public ILogger Logger { get; }
        public IClusterClient Cluster { get; }
        public string RoleName { get; set; }
        public bool IsRunning { get; protected set; }

        /// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
        public void Dispose()
        {
            if (IsRunning)
                Stop().Wait();
            Client?.Dispose();
            Logger.LogInformation($"{RoleName} test disposed");
        }

        public abstract Task Test();

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

        protected async Task UseRole()
        {
            await Client.ConnectAsync();
            await RegisterAndLogin();

            var rm = Client.Import<IRoleManager>();
            await rm.CreateRole(new CreateRoleOption {Gender = Gender.Male, Name = RoleName, Race = Race.Human});
            await rm.UseRole((await rm.GetRoleList()).First().Id);
        }
    }
}