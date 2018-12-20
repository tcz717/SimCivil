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
// SimCivil - SimCivil.Test - AppearanceTest.cs
// Create Date: 2018/12/19
// Update Date: 2018/12/19

using System;
using System.Text;
using System.Threading.Tasks;

using Orleans.TestingHost;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.System;

using Xunit;

namespace SimCivil.Test.Orleans
{
    [Collection(ClusterCollection.Name)]
    public class AppearanceTest
    {
        public AppearanceTest(OrleansFixture fixture)
        {
            Cluster = fixture.Cluster;
        }

        public TestCluster Cluster { get; }

        [Fact]
        public async Task UnitAppearance()
        {
            await Cluster.GrainFactory.GetGrain<IGame>(0).InitGame();
            IEntity unit = await Cluster.GrainFactory.GetGrain<IPrefabManager>(0).Clone("test", "human.init");
            await unit.Enable();
            IAppearance appearance = await unit.Get<IAppearance>();
            Assert.NotNull(appearance);

            AppearanceContainer container = await appearance.GetData();
            Assert.NotEmpty(container.Appearances);
        }
    }
}