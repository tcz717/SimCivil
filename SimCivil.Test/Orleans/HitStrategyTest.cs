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
// SimCivil - SimCivil.Test - HitStrategyTest.cs
// Create Date: 2019/06/03
// Update Date: 2019/06/03

using System;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using Orleans.Concurrency;

using SimCivil.Orleans.Grains.Strategy;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component.State;
using SimCivil.Orleans.Interfaces.Option;
using SimCivil.Orleans.Interfaces.Strategy;

using Xunit;
using Xunit.Abstractions;

namespace SimCivil.Test.Orleans
{
    [Collection(ClusterCollection.Name)]
    public class HitStrategyTest : OrleansBaseTest
    {
        public HitStrategyTest(OrleansFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

        [Fact]
        public async Task HitTest()
        {
            IEntity attackerEntity = await GetNewRoleAsync();
            IEntity defenderEntity = await GetNewRoleAsync();

            IHitStrategy hitStrategy = new TestHitStrategy(
                Cluster.GrainFactory,
                Options.Create(
                    new HitStrategyOptions
                    {
                        BodyHitBaseProbability = new[]
                        {
                            1f,
                            0f,
                            0f,
                            0f,
                            0f,
                            0f,
                            0f,
                            0f,
                            0f,
                            0f,
                            0f,
                            0f,
                            0f,
                            0f,
                            0f,
                            0f,
                            0f,
                            0f,
                            0f,
                        }
                    }));

            var attackResult = await hitStrategy.HitCalculateAsync(
                                            new Immutable<IEntity>(attackerEntity),
                                            new Immutable<IEntity>(defenderEntity),
                                            new Immutable<IEntity>(),
                                            HitMethod.Fist);

            Assert.NotNull(attackResult);
            if (!attackResult.IsEmpty)
            {
                Assert.Single(attackResult);
            }
        }
    }
}