using System;
using System.Linq;
using System.Net;
using System.Text;

using SimCivil.Contract;
using SimCivil.Rpc;

using Stateless;

namespace SimCivil.SimpleClient
{
    internal class SimCivilClient : IDisposable
    {
        public RpcClient RpcClient { get; private set; }

        protected StateMachine<ClientState, ClientTriger> ClientStateMachine { get; set; }

        protected StateMachine<ClientState, ClientTriger>.TriggerWithParameters<IPEndPoint> ConnectTriger { get; set; }

        public IAuth AuthService { get; set; }

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
                .Permit(ClientTriger.FetaError, ClientState.GetIpAndPort);
            ClientStateMachine.Configure(ClientState.LoggingIn)
                .SubstateOf(ClientState.Connected)
                .OnEntry(LogIn)
                .Permit(ClientTriger.LogInSuccess, ClientState.WaitCommand)
                .PermitReentry(ClientTriger.Retry);
            ClientStateMachine.Configure(ClientState.WaitCommand)
                .SubstateOf(ClientState.Connected)
                .PermitReentry(ClientTriger.Retry)
                .OnEntry(WaitInput);
        }

        private void WaitInput()
        {
            Console.Out.Write(">");
            string input = Console.ReadLine();

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
                        bool result = RoleManger.CreateRole(
                                new CreateRoleOption
                                {
                                    Name = args[1]
                                })
                            .Result;
                        Console.Out.WriteLine("Role create reuslt {0}", result ? "success" : "fail");

                        break;
                    case "lr":
                        var summaries = RoleManger.GetRoleList().Result;
                        for (var i = 0; i < summaries.Length; i++)
                        {
                            RoleSummary summary = summaries[i];
                            Console.Out.WriteLine($"summary[{i}] = {summary}");
                        }

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

        protected void LogIn()
        {
            Console.Write("User Name: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

            if (AuthService.LogIn(username, password))
            {
                RoleManger = RpcClient.Import<IRoleManger>();
                ClientStateMachine.Fire(ClientTriger.LogInSuccess);
            }
            else
            {
                ClientStateMachine.Fire(ClientTriger.Retry);
            }
        }

        public IRoleManger RoleManger { get; set; }

        protected void Connect(IPEndPoint endPoint)
        {
            RpcClient?.Dispose();
            RpcClient = new RpcClient(endPoint);
            try
            {
                RpcClient.ConnectAsync().Wait();
                AuthService = RpcClient.Import<IAuth>();
                ClientStateMachine.Fire(ClientTriger.LogIn);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                ClientStateMachine.Fire(ClientTriger.FetaError);
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
                ClientStateMachine.Fire(ClientTriger.Start);
        }

        internal enum ClientTriger
        {
            Start,
            Retry,
            Connect,
            FetaError,
            LogIn,
            LogInSuccess
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