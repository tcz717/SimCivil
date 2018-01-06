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
// SimCivil - SimCivil.SimpleClient - Program.cs
// Create Date: 2018/01/02
// Update Date: 2018/01/02

using System;
using System.Linq;
using System.Net;
using System.Text;

using SimCivil.Contract;
using SimCivil.Rpc;

using Stateless;

namespace SimCivil.SimpleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new SimCivilClient())
            {
                client.Run(args);
            }
        }
    }

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
                .PermitReentry(ClientTriger.Retry);
        }

        protected void LogIn()
        {
            Console.Write("User Name: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

            if (AuthService.LogIn(username, password))
            {
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