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
// SimCivil - SimCivil.Test - ChunkTest.cs
// Create Date: 2019/05/05
// Update Date: 2019/05/11

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Orleans;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Option;

using Xunit;
using Xunit.Abstractions;

namespace SimCivil.Test.Orleans
{
    [Collection(ClusterCollection.Name)]
    public class ChunkTest : OrleansBaseTest
    {
        [Fact]
        public async Task GetAtlasTest()
        {
            int atlasSize = Cluster.ServiceProvider.GetService<IOptions<GameOptions>>().Value.AtlasSize;
            for (int i = -5; i < 5; i++)
            for (int j = -5; j < 5; j++)
            {
                var atlas = Cluster.GrainFactory.GetGrain<IAtlas>((i, j));
                var tiles = await atlas.Dump();
                Assert.All(
                    tiles.Cast<Tile>(),
                    t =>
                    {
                        Assert.InRange(
                            t.Position.Y,
                            j * atlasSize,
                            (j + 1) * atlasSize - 1);
                        Assert.InRange(
                            t.Position.X,
                            i * atlasSize,
                            (i + 1) * atlasSize - 1);
                    });
            }
        }

        [Fact]
        public async Task MoveTest()
        {
            IEntity entity = await GetNewRoleAsync();

            var position = Cluster.GrainFactory.GetGrain<IComponent<Position>>(entity.GetPrimaryKey());
            var observer = Cluster.GrainFactory.GetGrain<IObserver>(entity.GetPrimaryKey());
            await position.SetData(new PositionState(100f, 100f));
            await observer.SetData(new ObserverState {NotifyRange = 5});

            IEntity testEntity = await GetNewRoleAsync();
            await testEntity.Enable();
            await testEntity.Add<IComponent<Position>>();
            await Cluster.GrainFactory.GetGrain<IComponent<Position>>(testEntity.GetPrimaryKey())
                         .SetData(new Position(100f, 100.5f));

            await Task.Delay(100);

            Assert.NotEmpty(await observer.PopAllEntities());
        }

        public ChunkTest(OrleansFixture fixture, ITestOutputHelper output) : base(fixture, output) { }
    }
}