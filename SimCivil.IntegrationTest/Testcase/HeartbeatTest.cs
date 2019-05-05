using Microsoft.Extensions.Logging;
using Orleans;
using SimCivil.Contract;
using SimCivil.Contract.Model;
using SimCivil.Orleans.Interfaces;
using SimCivil.Rpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Diagnostics.Debug;

namespace SimCivil.IntegrationTest.Testcase
{
    class HeartbeatTest : IIntegrationTest
    {
        private static int _id;
        protected RpcClient Client { get; private set; }
        public IClusterClient Cluster { get; }
        public bool IsRunning { get; private set; } = false;
        public ILogger<HeartbeatTest> Logger { get; }
        public string RoleName { get; set; }

        public HeartbeatTest(ILogger<HeartbeatTest> logger, IClusterClient cluster)
        {
            RoleName = nameof(HeartbeatTest) + Interlocked.Increment(ref _id);
            Logger = logger;
            Cluster = cluster;
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
            throw new NotImplementedException();
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
            await rm.CreateRole(new CreateRoleOption { Gender = Gender.Male, Name = RoleName, Race = Race.Human });
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
            if (name == null)
            {
                name = RoleName;
            }

            await Cluster.GetGrain<IAccount>(name).Register(password);
            Logger.LogInformation($"Role \"{RoleName}\" Created");
            await Login(name, password);
        }

        protected async Task Login(string name = null, string password = "")
        {
            if (name == null)
            {
                name = RoleName;
            }

            var auth = Client.Import<IAuth>();
            await auth.LogInAsync(name, password);

            Logger.LogInformation($"Role \"{RoleName}\" login");
        }
    }
}
