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
// Update Date: 2018/12/02

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.Extensions.Logging;

using Orleans;

using SimCivil.Contract;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Rpc;
using SimCivil.Rpc.Session;

namespace SimCivil.Gate
{
    internal class OrleansChunkViewSynchronizer : IViewSynchronizer, ISessionRequired
    {
        public IGrainFactory GrainFactory { get; }
        public ILogger<IViewSynchronizer> Logger { get; }

        static OrleansChunkViewSynchronizer()
        {
            Mapper.Initialize(c =>
            {
                c.CreateMap<Tile, TileDto>();
                c.CreateMap<AppearanceEntry, AppearanceDto>();
            });
        }

        public OrleansChunkViewSynchronizer(IGrainFactory grainFactory, ILogger<IViewSynchronizer> logger)
        {
            GrainFactory = grainFactory;
            Logger = logger;
        }

        public AsyncLocal<IRpcSession> Session { get; set; } = new AsyncLocal<IRpcSession>();

        public void RegisterViewSync(Action<ViewChange> callback)
        {
            Logger.LogInformation("Registered callback {0}", callback.Target);
            Session.Value.Set(callback);
        }

        public void DeregisterViewSync()
        {
            Session.Value.UnSet<Action<ViewChange>>();
        }

        public async Task<TileDto[]> GetAtlas((int X, int Y) index)
        {
            var tiles = await GrainFactory.GetGrain<IAtlas>(index).Dump();

            return Mapper.Map<IEnumerable<Tile>, TileDto[]>(tiles.Cast<Tile>());
        }

        public async Task<DateTime> GetAtlasTimeStamp((int X, int Y) index)
        {
            DateTime timeStamp = await GrainFactory.GetGrain<IAtlas>(index).GetTimeStamp();

            return timeStamp;
        }

        /// <summary>Gets the appearance.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">appearance</exception>
        public async Task<AppearanceDto[]> GetAppearance(Guid entity)
        {
            IAppearance appearance = await Session.Value.Get<IEntity>().Get<IAppearance>();

            if (appearance == null) throw new ArgumentNullException(nameof(appearance));

            return Mapper.Map<AppearanceDto[]>((await appearance.GetData()).Appearances);
        }

        public void StartSync(RpcServer server)
        {
            Task.Run(
                () =>
                {
                    while (true)
                    {
                        var sessions = server.Sessions.Where(s => s.IsSet<Action<ViewChange>>()).ToArray();
                        Logger.LogDebug($"Start updating {sessions.Length} clients");
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