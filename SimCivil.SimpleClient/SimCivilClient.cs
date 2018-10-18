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
// SimCivil - SimCivil.SimpleClient - SimCivilClient.cs
// Create Date: 2018/01/07
// Update Date: 2018/02/22

using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using SimCivil.Contract;
using SimCivil.Rpc;

using Stateless;

namespace SimCivil.SimpleClient
{
    internal class SimCivilClient : IDisposable
    {
        private RoleSummary[] _roleList;
        public RpcClient RpcClient { get; private set; }

        protected StateMachine<ClientState, ClientTriger> ClientStateMachine { get; set; }

        protected StateMachine<ClientState, ClientTriger>.TriggerWithParameters<IPEndPoint> ConnectTriger { get; set; }

        public IAuth AuthService { get; set; }

        public IViewSynchronizer Synchronizer { get; set; }

        public IRoleManager RoleManager { get; set; }

        public SimCivilClient()
        {
            ClientStateMachine = new StateMachine<ClientState, ClientTriger>(ClientState.Disconnected);
            ConfigureStateMachine();
        }

        public void Dispose()
        {
            RpcClient?.Dispose();
        }

        private void ConfigureStateMachine()
        {
            ConnectTriger = ClientStateMachine.SetTriggerParameters<IPEndPoint>(ClientTriger.Connect);
            ClientStateMachine.Configure(ClientState.Disconnected)
                .Permit(ClientTriger.Start, ClientState.GetIpAndPort)
                .Permit(ClientTriger.Connect, ClientState.Connecting);
            ClientStateMachine.Configure(ClientState.GetIpAndPort)
                .SubstateOf(ClientState.Disconnected)
                .OnEntry(GetIpAndPort)
                .PermitReentry(ClientTriger.Retry)
                .Permit(ClientTriger.Connect, ClientState.Connecting);
            ClientStateMachine.Configure(ClientState.Connecting)
                .SubstateOf(ClientState.Disconnected)
                .OnEntryFrom(ConnectTriger, Connect)
                .Permit(ClientTriger.LogIn, ClientState.LoggingIn)
                .Permit(ClientTriger.FetalError, ClientState.GetIpAndPort);
            ClientStateMachine.Configure(ClientState.LoggingIn)
                .SubstateOf(ClientState.Connected)
                .OnEntryAsync(LogIn)
                .Permit(ClientTriger.LogInSuccess, ClientState.WaitCommand)
                .PermitReentry(ClientTriger.Retry);
            ClientStateMachine.Configure(ClientState.WaitCommand)
                .SubstateOf(ClientState.Connected)
                .PermitReentry(ClientTriger.Retry)
                .Permit(ClientTriger.LostConnection, ClientState.GetIpAndPort)
                .OnEntry(WaitInput);
        }

        private void WaitInput()
        {
            Console.Out.Write(">");
            string input = Console.ReadLine();

            if (!(RpcClient?.Channel.Active ?? false))
            {
                Console.Out.WriteLine("Lost connection...");
                ClientStateMachine.Fire(ClientTriger.LostConnection);
            }

            if (input == null)
            {
                ClientStateMachine.Fire(ClientTriger.Retry);

                return;
            }

            var args = input.Split();
            try
            {
                switch (args[0].ToLower())
                {
                    case "cr":
                        bool result = RoleManager.CreateRole(
                                new CreateRoleOption
                                {
                                    Name = args[1]
                                })
                            .Result;
                        Console.Out.WriteLine("Role create reuslt {0}", result ? "success" : "fail");

                        break;
                    case "lr":
                        var summaries = RoleManager.GetRoleList().Result;
                        _roleList = summaries;
                        for (var i = 0; i < summaries.Length; i++)
                        {
                            RoleSummary summary = summaries[i];
                            Console.Out.WriteLine($"summary[{i}] = {summary}");
                        }

                        break;
                    case "s":
                        if (!int.TryParse(args[1], out int index))
                        {
                            Console.Out.WriteLine("Wrong index");

                            return;
                        }

                        result = RoleManager.UseRole(_roleList[index].Id).Result;
                        Console.Out.WriteLine("result = {0}", result);
                        if (result)
                            RegisterSync();

                        break;
                    default:
                        Console.Out.WriteLine("Unknown cmd. Please check your input.");

                        return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            ClientStateMachine.Fire(ClientTriger.Retry);
        }

        private void RegisterSync()
        {
            Synchronizer = RpcClient.Import<IViewSynchronizer>();
            Synchronizer.RegisterViewSync(
                change =>
                {
                    if (change.EntityChange != null)
                        foreach (EntityDto entityDto in change.EntityChange)
                        {
                            Console.Out.WriteLine(entityDto);
                        }

                    if (change.Events != null)
                        foreach (ViewEvent viewEvent in change.Events)
                        {
                            Console.Out.WriteLine(viewEvent);
                        }
                    if (change.TileChange != null)
                        foreach (TileDto tileDto in change.TileChange)
                        {
                            Console.Out.WriteLine(tileDto);
                        }
                });
        }

        protected async Task LogIn()
        {
            Console.Write("User Name: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

            if (await AuthService.LogInAsync(username, password))
            {
                RoleManager = RpcClient.Import<IRoleManager>();
                ClientStateMachine.Fire(ClientTriger.LogInSuccess);
            }
            else
            {
                ClientStateMachine.Fire(ClientTriger.Retry);
            }
        }

        protected void Connect(IPEndPoint endPoint)
        {
            RpcClient?.Dispose();
            RpcClient = new RpcClient(endPoint) {ResponseTimeout = 10000};
            try
            {
                RpcClient.ConnectAsync().Wait();
                AuthService = RpcClient.Import<IAuth>();
                ClientStateMachine.Fire(ClientTriger.LogIn);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                ClientStateMachine.Fire(ClientTriger.FetalError);
            }
        }

        private void GetIpAndPort()
        {
            Console.WriteLine("Please input the server IP and port");
            Console.Write("IP (127.0.0.1): ");
            string sip = Console.ReadLine();
            Console.Write("Port (20170): ");
            string sport = Console.ReadLine();

            sip = string.IsNullOrWhiteSpace(sip) ? IPAddress.Loopback.ToString() : sip;
            sport = string.IsNullOrWhiteSpace(sport) ? "20170" : sport;
            if (sip == null || (!IPAddress.TryParse(sip, out IPAddress ip) || !ushort.TryParse(sport, out ushort port)))
            {
                Console.WriteLine("Arguments format error, please try again.");
                ClientStateMachine.Fire(ClientTriger.Retry);

                return;
            }

            Console.Clear();
            ClientStateMachine.Fire(ConnectTriger, new IPEndPoint(ip, port));
        }

        public void Run(string[] args)
        {
            if (args.Any(a => a.Trim() == "test"))
                ClientStateMachine.Fire(ConnectTriger, new IPEndPoint(IPAddress.Loopback, 20170));
            else
                ClientStateMachine.FireAsync(ClientTriger.Start).Wait();
        }

        internal enum ClientTriger
        {
            Start,
            Retry,
            Connect,
            FetalError,
            LogIn,
            LogInSuccess,
            LostConnection
        }

        internal enum ClientState
        {
            Disconnected,
            GetIpAndPort,
            Connecting,
            Connected,
            LoggingIn,
            WaitCommand
        }
    }
}