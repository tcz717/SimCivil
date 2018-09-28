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
// Create Date: 2018/06/14
// Update Date: 2018/06/16

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;

namespace SimCivil.Orleans.Grains.Component
{
    public class PositionGrain : BaseGrain<Position>, IPosition
    {
        private static readonly (int X, int Y)[] Offsets =
            {(0, 0), (1, 0), (0, 1), (-1, 0), (0, -1), (1, 1), (-1, -1), (1, -1), (-1, 1)};

        public override async Task SetData(Position component)
        {
            if (!component.Equals(State))
            {
                (int X, int Y) prevChunk = (State ?? component).Tile.DivDown(Config.ChunkSize);
                (int X, int Y) currentChunk = component.Tile.DivDown(Config.ChunkSize);
                var effectChunks = Offsets.Select(o => (prevChunk.X + o.X, prevChunk.Y + o.Y))
                    .Union(Offsets.Select(o => (currentChunk.X + o.X, currentChunk.Y + o.Y)));

                Guid entityGuid = this.GetPrimaryKey();
                await Task.WhenAll(
                    effectChunks.Select(
                        chunk => GrainFactory.GetGrain<IChunk>(chunk)
                            .OnEntityMoved(entityGuid, (State ?? component), component)));
            }

            await base.SetData(component);
        }

        public async Task<Position> Add((int X, int Y) offset)
        {
            Position pos = await GetData();
            pos += offset;
            await SetData(pos);

            return pos;
        }
    }
}