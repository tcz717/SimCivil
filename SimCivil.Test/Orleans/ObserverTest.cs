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
// SimCivil - SimCivil.Test - ObserverTest.cs
// Create Date: 2019/05/05
// Update Date: 2019/05/31

using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

using Orleans;

using SimCivil.Contract;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;

using Xunit;
using Xunit.Abstractions;

namespace SimCivil.Test.Orleans
{
    [Collection(ClusterCollection.Name)]
    public class ObserverTest : OrleansBaseTest
    {
        [Fact]
        public void DumpTest()
        {
            var tiles = Cluster.GrainFactory.GetGrain<IAtlas>(0).Dump().Result;

            Assert.NotEmpty(tiles);
        }

        [Fact]
        public async Task UpdateViewTest()
        {
            IEntity entity = await GetNewRoleAsync();

            var position = Cluster.GrainFactory.GetGrain<IComponent<PositionState>>(entity.GetPrimaryKey());
            var observer = Cluster.GrainFactory.GetGrain<IObserver>(entity.GetPrimaryKey());
            await position.SetData(new PositionState(100f, 100f));

            IEntity testEntity = await GetNewRoleAsync();
            await testEntity.Enable();
            await testEntity.Add<IComponent<PositionState>>();
            await Cluster.GrainFactory.GetGrain<IComponent<PositionState>>(testEntity.GetPrimaryKey())
                         .SetData(new PositionState(100f, 100.5f));

            await Task.Delay(100);

            ViewChange viewChange = await observer.UpdateView();

            Assert.NotNull(viewChange.EntityChange);
            Assert.NotEmpty(viewChange.EntityChange);
            Assert.Contains(viewChange.EntityChange, e => e.MaxSpeed != null && e.Hp != null);
        }

        public ObserverTest(OrleansFixture fixture, ITestOutputHelper output) : base(fixture, output) { }
    }
}