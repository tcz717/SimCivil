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
// Create Date: 2018/05/12
// Update Date: 2018/05/17

using System;
using System.Text;
using System.Threading.Tasks;

using Orleans;
using Orleans.TestingHost;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;

using Xunit;

namespace SimCivil.Test.Orleans
{
    public class ChunkTest
    {
        public ChunkTest()
        {
            Cluster = OrleansFixture.Single.Cluster;
        }

        public TestCluster Cluster { get; }

        [Fact]
        public async Task MoveTest()
        {
            var entity = Cluster.GrainFactory.GetGrain<IEntity>(Guid.NewGuid());
            await entity.Enable();
            await entity.Add<IComponent<Position>>();
            await entity.Add<IObserver>();
            var position = Cluster.GrainFactory.GetGrain<IComponent<Position>>(entity.GetPrimaryKey());
            var observer = Cluster.GrainFactory.GetGrain<IObserver>(entity.GetPrimaryKey());
            await position.SetData(new Position(100f, 100f));
            await observer.SetData(new Observer {NotityRange = 5});

            var testEntity = Cluster.GrainFactory.GetGrain<IEntity>(Guid.NewGuid());
            await testEntity.Enable();
            await testEntity.Add<IComponent<Position>>();
            await Cluster.GrainFactory.GetGrain<IComponent<Position>>(testEntity.GetPrimaryKey())
                .SetData(new Position(100f, 104f));

            await Task.Delay(100);

            Assert.NotEmpty(await observer.PopAllEntities());
        }
    }
}