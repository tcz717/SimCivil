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
// SimCivil - SimCivil.Test - UnitTest.cs
// Create Date: 2018/12/31
// Update Date: 2018/12/31

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Orleans.TestingHost;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;

using Xunit;
using Xunit.Abstractions;

namespace SimCivil.Test.Orleans
{
    [Collection(ClusterCollection.Name)]
    public class UnitTest
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public UnitTest(OrleansFixture fixture, ITestOutputHelper output)
        {
            Output = output;
            Cluster = fixture.Cluster;
        }

        public ITestOutputHelper Output { get; }
        public TestCluster Cluster { get; }

        [Fact]
        public async Task InspectTest()
        {
            var unit = Cluster.GrainFactory.GetGrain<IUnit>(Guid.NewGuid());
            var inspectDict = await unit.Inspect(Cluster.GrainFactory.GetEntity(unit));
            Assert.NotEmpty(inspectDict);
            foreach (var keyValuePair in inspectDict)
            {
                Output.WriteLine($"[{keyValuePair.Key}] = {keyValuePair.Value}");
            }
        }
    }
}