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
// SimCivil - SimCivil.Orleans.Grains - ChunkGrain.cs
// Create Date: 2018/06/14
// Update Date: 2018/12/18

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Orleans;
using Orleans.Runtime;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Option;

using static System.Math;

namespace SimCivil.Orleans.Grains
{
    public class ChunkGrain : Grain, IChunk
    {
        public Dictionary<Guid, Position> Entities;

        public ILogger<ChunkGrain> Logger { get; }
        public IOptions<SyncOptions> SyncOptions { get; }

        /// <summary>
        /// This constructor should never be invoked. We expose it so that client code (subclasses of Grain) do not have to add a constructor.
        /// Client code should use the GrainFactory property to get a reference to a Grain.
        /// </summary>
        public ChunkGrain(ILogger<ChunkGrain> logger, IOptions<SyncOptions> syncOptions)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            SyncOptions = syncOptions;
        }

        public async Task OnEntityMoved(Guid entityGuid, Position previousPos, Position currentPos)
        {
            Logger.Debug($"{entityGuid} entity has moved");

            var movedEntity = GrainFactory.GetGrain<IEntity>(entityGuid);
            foreach (var e in Entities)
            {
                if (entityGuid == e.Key) continue;

                var entity = GrainFactory.GetGrain<IEntity>(e.Key);

                float prevDistance = Max(
                    Abs(previousPos.X - e.Value.X),
                    Abs(previousPos.Y - e.Value.Y));
                float currentDistance = Max(Abs(currentPos.X - e.Value.X), Abs(currentPos.Y - e.Value.Y));

                if (await movedEntity.Has<IObserver>())
                {
                    var movedObserver = GrainFactory.Get<IObserver>(movedEntity);
                    uint range = await movedObserver.GetNotifyRange();

                    if (currentDistance < range)
                    {
                        await movedObserver.OnEntityEntered(e.Key);
                    }
                    else if (prevDistance < range)
                    {
                        await movedObserver.OnEntityLeft(e.Key);
                    }
                }

                if (await entity.Has<IObserver>())
                {
                    var effectedObserver = GrainFactory.Get<IObserver>(entity);
                    uint range = await effectedObserver.GetNotifyRange();

                    if (currentDistance < range)
                    {
                        await effectedObserver.OnEntityEntered(entityGuid);
                    }
                    else if (prevDistance < range)
                    {
                        await effectedObserver.OnEntityLeft(entityGuid);
                    }
                }
            }

            (int X, int Y) currentChunk = currentPos.Tile.DivDown(SyncOptions.Value.ChunkSize);
            (int X, int Y) thisChunk = this.GetPrimaryKeyXY();
            if (currentChunk.X == thisChunk.X && currentChunk.Y == thisChunk.Y)
            {
                Entities[entityGuid] = currentPos;
            }
            else
            {
                Entities.Remove(entityGuid);
            }
        }

        /// <summary>Called when [tile changed].</summary>
        /// <param name="tile">The tile.</param>
        /// <returns></returns>
        public async Task OnTileChanged(Tile tile)
        {
            await Task.WhenAll(
                Entities.Keys.Select(
                    id => GrainFactory.GetEntity(id)
                        .Get<IObserver>()
                        .ContinueWith(o => { o.Result?.OnTileChanged(tile); })));
        }

        public override Task OnActivateAsync()
        {
            Entities = new Dictionary<Guid, Position>();

            return base.OnActivateAsync();
        }
    }
}