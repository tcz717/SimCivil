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
// SimCivil - SimCivil.Gate - OrleansAuth.cs
// Create Date: 2018/06/14
// Update Date: 2018/06/17

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

using Orleans;

using SimCivil.Contract;
using SimCivil.Orleans.Interfaces;
using SimCivil.Rpc;
using SimCivil.Rpc.Session;

namespace SimCivil.Gate
{
    public class OrleansAuth : IAuth, ISessionRequired
    {
        public IGrainFactory GrainFactory { get; }
        public ILogger<IAuth> Logger { get; }

        public OrleansAuth(IGrainFactory grainFactory, ILogger<IAuth> logger)
        {
            GrainFactory = grainFactory;
            Logger = logger;
        }

        public bool LogIn(string username, string password)
        {
            throw new NotSupportedException();
        }

        public async Task<bool> LogInAsync(string username, string password)
        {
            var account = GrainFactory.GetGrain<IAccount>(username);

            Logger.LogInformation($"{DateTime.Now} try login");
            if (!await account.Login(password))
            {
                Logger.LogWarning($"{DateTime.Now} login fail");

                return false;
            }

            Session.Value.Set(account).Exiting += Session_Exiting;
            Logger.LogInformation($"{DateTime.Now} login success");

            return true;
        }

        public void LogOut()
        {
            throw new NotSupportedException();
        }

        public async Task LogOutAsync()
        {
            if (Session.Value.IsSet<IAccount>())
            {
                await Session.Value.Get<IAccount>().Logout();
                Session.Value.UnSet<IAccount>();
            }
        }

        public string GetToken()
        {
            throw new NotSupportedException();
        }

        public async Task<bool> Register(string username, string password)
        {
            if (username == null) return false;
            if (password == null) return false;

            var account = GrainFactory.GetGrain<IAccount>(username);

            return await account.Register(password);
        }

        public AsyncLocal<IRpcSession> Session { get; set; } = new AsyncLocal<IRpcSession>();

        private void Session_Exiting(object sender, EventArgs e)
        {
            var session = (IRpcSession) sender;
            session.Exiting -= Session_Exiting;
            if (session.IsSet<IAccount>())
                session.Get<IAccount>().Logout();
        }
    }
}