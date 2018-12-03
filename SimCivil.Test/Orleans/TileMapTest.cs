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
// SimCivil - SimCivil.Test - TileMapTest.cs
// Create Date: 2018/05/14
// Update Date: 2018/05/14

using System;
using System.Linq;
using System.Text;

using Orleans.TestingHost;

using SimCivil.Orleans.Interfaces;

using Xunit;

namespace SimCivil.Test.Orleans
{
    [Collection(ClusterCollection.Name)]
    public class TileMapTest
    {
        public TestCluster Cluster { get; set; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public TileMapTest(OrleansFixture fixture)
        {
            Cluster = fixture.Cluster;
        }

        [Fact]
        public void MapGeneratingTest()
        {
            var atlas = Cluster.GrainFactory.GetGrain<IAtlas>(0x0000_FFFF_0000_FFFF);
            var tiles = atlas.SelectRange((0x0000_FFFF * 64, 0x0000_FFFF * 64), 64, 64).Result.ToArray();
            Assert.Equal(64 * 64, tiles.Length);
        }

        [Fact]
        public void TimeStampTest()
        {
            var atlas = Cluster.GrainFactory.GetGrain<IAtlas>(0);
            DateTime init = atlas.GetTimeStamp().Result;
            Assert.Equal(init,atlas.GetTimeStamp().Result);
            atlas.SetTile(Tile.Create((0, 0), 2));
            Assert.NotEqual(init, atlas.GetTimeStamp().Result);
        }

        /*       [TestMethod]
        public void SelectRangeTest()
        {
            AtlasGrain atlas =
                A.Fake<AtlasGrain>(a => a.WithArgumentsForConstructor(() => new AtlasGrain(new RandomMapGen())).CallsBaseMethods());
            A.CallTo(()=>atlas.OnActivateAsync())
        }*/
    }
}