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
// SimCivil - SimCivil.Gate - OrleansChunkViewSynchronizer.cs
// Create Date: 2018/06/16
// Update Date: 2018/06/17

using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Runtime;

using SimCivil.Contract;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Rpc;
using SimCivil.Rpc.Session;

namespace SimCivil.Gate
{
    internal class OrleansChunkViewSynchronizer : IViewSynchronizer, ISessionRequred
    {
        public IGrainFactory GrainFactory { get; }
        public ILogger<IViewSynchronizer> Logger { get; }

        public OrleansChunkViewSynchronizer(IGrainFactory grainFactory, ILogger<IViewSynchronizer> logger)
        {
            GrainFactory = grainFactory;
            Logger = logger;
        }

        public AsyncLocal<IRpcSession> Session { get; set; } = new AsyncLocal<IRpcSession>();

        public void RegisterViewSync(Action<ViewChange> callback)
        {
            Session.Value.Set(callback);
        }

        public void StartSync(RpcServer server)
        {
            Task.Run(
                () =>
                {
                    while (true)
                    {
                        var sessions = server.Sessions.Where(s => s.IsSet<Action<ViewChange>>()).ToArray();
                        Logger.Info($"Start updating {sessions.Length} clients");
                        foreach (IRpcSession session in sessions)
                        {
                            ViewChange viewChange =
                                GrainFactory.Get<IObserver>(session.Get<IEntity>()).UpdateView().Result;
                            session.Get<Action<ViewChange>>()(viewChange);
                        }

                        Thread.Sleep(50);
                    }
                });
        }
    }
}