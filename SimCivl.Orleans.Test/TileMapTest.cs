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
// SimCivil - SimCivl.Orleans.Test - TileMapTest.cs
// Create Date: 2018/02/26
// Update Date: 2018/02/26

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

using FakeItEasy;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using Orleans.TestingHost;

using SimCivil.Orleans.Grains;
using SimCivil.Orleans.Grains.Service;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Service;

namespace SimCivl.Orleans.Test
{
    [TestClass]
    public class TileMapTest
    {
        public TestCluster Cluster { get; set; }
        public TestContext Context { get; set; }

        [TestInitialize]
        public void Init()
        {
            Cluster = new TestCluster();
            Cluster.ClusterConfiguration.AddMemoryStorageProvider();
            Cluster.UseSiloBuilderFactory<MockFactory>();
            Cluster.Deploy();
        }

        [TestMethod]
        public void MapGeneratingTest()
        {
            var atlas = Cluster.GrainFactory.GetGrain<IAtlas>(0x0000_FFFF_0000_FFFF);
            var tiles = atlas.SelectRange((0x0000_FFFF * 64, 0x0000_FFFF * 64), 64, 64).Result.ToArray();
            Assert.AreEqual(64 * 64, tiles.Length);
        }

 /*       [TestMethod]
        public void SelectRangeTest()
        {
            AtlasGrain atlas =
                A.Fake<AtlasGrain>(a => a.WithArgumentsForConstructor(() => new AtlasGrain(new RandomMapGen())).CallsBaseMethods());
            A.CallTo(()=>atlas.OnActivateAsync())
        }*/
    }

    public class MockFactory : ISiloBuilderFactory
    {
        public ISiloHostBuilder CreateSiloBuilder(string siloName, ClusterConfiguration clusterConfiguration)
        {
            return new SiloHostBuilder()
                .ConfigureSiloName(siloName)
                .UseConfiguration(clusterConfiguration)
                .ConfigureLogging(
                    logging => logging.AddConsole()
                        .AddDebug())
                .ConfigureServices(
                    services => { services.AddSingleton<IMapGenerator, RandomMapGen>(); });
        }
    }
}