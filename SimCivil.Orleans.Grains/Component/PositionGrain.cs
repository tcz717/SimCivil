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
// SimCivil - SimCivil.Orleans.Grains - PositionGrain.cs
// Create Date: 2019/05/13
// Update Date: 2019/05/31

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Orleans;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Option;
using SimCivil.Orleans.Interfaces.Service;

namespace SimCivil.Orleans.Grains.Component
{
    [PublicAPI]
    public class PositionGrain : BaseGrain<PositionState>, IPosition
    {
        public ITerrainRepository    TerrainRepository { get; }
        public IOptions<SyncOptions> SyncOptions       { get; }

        public PositionGrain(
            ILoggerFactory        factory,
            ITerrainRepository    terrainRepository,
            IOptions<SyncOptions> syncOptions) : base(factory)
        {
            TerrainRepository = terrainRepository;
            SyncOptions       = syncOptions;
        }

        public override async Task SetData(PositionState component)
        {
            if (!component.Equals(State))
            {
                (int X, int Y) prevChunk = (State ?? component).Tile.DivDown(SyncOptions.Value.ChunkSize);
                (int X, int Y) currentChunk = component.Tile.DivDown(SyncOptions.Value.ChunkSize);
                var effectChunks = prevChunk.ForAround().Union(currentChunk.ForAround());

                Guid entityGuid = this.GetPrimaryKey();
                await Task.WhenAll(
                    effectChunks.Select(
                        chunk => GrainFactory.GetGrain<IChunk>(chunk)
                                             .OnEntityMoved(entityGuid, State ?? component, component)));
            }

            await base.SetData(component);
        }

        public async Task<PositionState> Add((float X, float Y) offset)
        {
            PositionState pos = await GetData();
            pos += offset;
            await SetData(pos);

            return pos;
        }

        public override Task<IReadOnlyDictionary<string, string>> Dump() => Task.FromResult(
            (IReadOnlyDictionary<string, string>) new Dictionary<string, string>
            {
                ["Position"] = State.ToString()
            });
    }
}