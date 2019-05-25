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
// SimCivil - SimCivil.IntegrationTest - HeartbeatTest.cs
// Create Date: 2019/05/08
// Update Date: 2019/05/19

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
    internal class HeartbeatTest : IIntegrationTest
    {
        private static int                    _id;
        protected      RpcClient              Client    { get; private set; }
        public         IClusterClient         Cluster   { get; }
        public         bool                   IsRunning { get; private set; }
        public         ILogger<HeartbeatTest> Logger    { get; }
        public         string                 RoleName  { get; set; }

        public HeartbeatTest(ILogger<HeartbeatTest> logger, IClusterClient cluster)
        {
            RoleName = nameof(HeartbeatTest) + Interlocked.Increment(ref _id);
            Logger   = logger;
            Cluster  = cluster;
        }

        private void NewClient()
        {
            Client = new RpcClient(
                new IPEndPoint(
                    Dns.GetHostAddresses(Dns.GetHostName()).First(ip => ip.AddressFamily == AddressFamily.InterNetwork),
                    20170));
        }

        public Guid GetEntityId()
        {
            var rm = Client.Import<IRoleManager>();

            return Task.Factory.StartNew(() => rm.GetRoleList().Result).Result.First().Id;
        }

        public Task Stop()
        {
            Client.Disconnect();
            IsRunning = false;

            return Task.CompletedTask;
        }

        public async Task Test()
        {
            IsRunning = true;
            NewClient();
            await Client.ConnectAsync();
            await RegisterAndLogin();

            // Create a client with heartbeat and wait
            var rm = Client.Import<IRoleManager>();
            await rm.CreateRole(new CreateRoleOption {Gender = Gender.Male, Name = RoleName, Race = Race.Human});
            await rm.UseRole((await rm.GetRoleList()).First().Id);

            await Task.Delay(15000);
            Client.Disconnect();
            Client.Dispose();
            Logger.LogInformation($"{RoleName} client disconnected");
            await Task.Delay(1000);

            // Create a client without heartbeat and wait
            NewClient();
            Client.HeartbeatDelay = int.MaxValue;
            await Client.ConnectAsync();
            await Login();

            rm = Client.Import<IRoleManager>();
            await rm.UseRole((await rm.GetRoleList()).First().Id);

            await Task.Delay(10000);
            Client.Disconnect();
            Client.Dispose();
            Logger.LogInformation($"{RoleName} client disconnected");
            await Task.Delay(1000);

            Logger.LogInformation($"{RoleName} test end");
        }

        protected async Task RegisterAndLogin(string name = null, string password = "")
        {
            name = name ?? RoleName;
            await Cluster.GetGrain<IAccount>(name).Register(password);
            Logger.LogInformation($"Role \"{RoleName}\" Created");
            await Login(name, password);
        }

        protected async Task Login(string name = null, string password = "")
        {
            var auth = Client.Import<IAuth>();
            await auth.LogInAsync(name ?? RoleName, password);

            Logger.LogInformation($"Role \"{RoleName}\" login");
        }
    }
}