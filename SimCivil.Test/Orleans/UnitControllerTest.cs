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
// SimCivil - SimCivil.Test - UnitControllerTest.cs
// Create Date: 2019/04/25
// Update Date: 2019/04/29

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Option;

using Xunit;
using Xunit.Abstractions;

namespace SimCivil.Test.Orleans
{
    [Collection(ClusterCollection.Name)]
    [SuppressMessage("ReSharper", "ImplicitlyCapturedClosure")]
    public class UnitControllerTest : OrleansBaseTest
    {
        public UnitControllerTest(OrleansFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public async Task MoveTest(int roleNum)
        {
            const double eps = 0.01;
            const int steps = 50;

            var syncOptions = Cluster.ServiceProvider.GetService<IOptions<SyncOptions>>();
            Assert.True(syncOptions.Value.UpdatePeriod > 0, "UpdatePeriod > 0");

            var roles = (await GetNewRolesAsync(roleNum)).ToArray();

            var positions = roles.Select(Cluster.GrainFactory.Get<IPosition>).ToArray();
            var units = roles.Select(Cluster.GrainFactory.Get<IUnit>).ToArray();
            var unitControllers = roles.Select(Cluster.GrainFactory.Get<IUnitController>).ToArray();

            float[] deltas = await Task.WhenAll(
                units.Select(async u => await u.GetMoveSpeed() * syncOptions.Value.UpdatePeriod / 1000));
            Assert.All(deltas, d => Assert.True(d > 0));

            PositionState[] initPos = await Task.WhenAll(positions.Select(s => s.GetData()));
            for (int i = 0; i < steps; i++)
            {
                PositionState[] posArr = new PositionState[roleNum];
                Task[] tasks = new Task[roleNum];
                for (int j = 0; j < roleNum; j++)
                {
                    posArr[j] = initPos[j] + (i * deltas[j], 0);
                    tasks[j] = unitControllers[j].MoveTo(posArr[j], DateTime.UtcNow);
                }

                await Task.WhenAll(tasks);
                await Task.Delay(syncOptions.Value.UpdatePeriod);

                for (int j = 0; j < roleNum; j++)
                {
                    (float actualX, float actualY) = await positions[j].GetData();
                    Assert.InRange(posArr[j].X - actualX, -eps, eps);
                    Assert.InRange(posArr[j].Y - actualY, -eps, eps);
                }
            }
        }
    }
}