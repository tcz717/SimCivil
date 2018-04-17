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
// SimCivil - SimCivil.Test - ViewSynchronizerTest.cs
// Create Date: 2018/02/05
// Update Date: 2018/02/08

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

using Autofac;

using SimCivil.Components;
using SimCivil.Contract;
using SimCivil.Map;
using SimCivil.Model;
using SimCivil.Store;
using SimCivil.Sync;

using Xunit;
using Xunit.Abstractions;

namespace SimCivil.Test
{
    public class ViewSynchronizerTest
    {
        public ViewSynchronizerTest(ITestOutputHelper output)
        {
            Output = output;
            var builder = new ContainerBuilder();
            builder.RegisterType<LocalEntityManager>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<TileMap>();
            builder.RegisterType<MemoryMapRepo>().AsImplementedInterfaces();
            builder.RegisterType<RandomMapGen>().AsImplementedInterfaces();
            builder.RegisterType<ChunkViewSynchronizer>();
            var container = builder.Build();
            Entities = container.Resolve<IEntityManager>();
            Synchronizer = container.Resolve<ChunkViewSynchronizer>();
        }

        public ITestOutputHelper Output { get; }
        public ChunkViewSynchronizer Synchronizer { get; set; }

        public IEntityManager Entities { get; set; }

        [Theory, Trait("Category", "Performance")]
        [InlineData(50, 100, 100, 10)]
        [InlineData(100, 50, 100, 10)]
        [InlineData(50, 100, 1000, 10)]
        [InlineData(50, 100, 10, 10)]
        [InlineData(50, 100, 100, 100)]
        [InlineData(1000, 5000, 5000, 200)]
        public void ChunkSyncPerformanceTest(
            int observerCount,
            int nonObserverCount,
            int maxPos,
            int changePerTick)
        {
            Output.WriteLine(
                "{0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}",
                nameof(observerCount),
                observerCount,
                nameof(nonObserverCount),
                nonObserverCount,
                nameof(maxPos),
                maxPos,
                nameof(changePerTick),
                changePerTick);
            const int testNum = 500;
            int minValue = -maxPos;
            int entityChangeCount = 0;

            Entity observer = Entity.Create()
                .Add<PositionComponent>()
                .Add<ObserverComponent>(
                    c =>
                    {
                        c.NotityRange = 5;
                        c.Callback = vc => entityChangeCount += vc.EntityChange?.Length ?? 0;
                    });
            Entity nonObserver = Entity.Create().Add<PositionComponent>();

            Random random = new Random();
            for (int i = 0; i < observerCount; i++)
            {
                Entity entity = observer.Clone();
                entity.GetPos().Pos = (random.Next(minValue, maxPos), random.Next(minValue, maxPos));
                Entities.AttachEntity(entity);
            }

            for (int i = 0; i < nonObserverCount; i++)
            {
                Entity entity = nonObserver.Clone();
                entity.GetPos().Pos = (random.Next(minValue, maxPos), random.Next(minValue, maxPos));
                Entities.AttachEntity(entity);
            }

            var entities = Entities.All.Entities.ToImmutableArray();

            Stopwatch stopwatch = new Stopwatch();
            for (int i = 0; i < testNum; i++)
            {
                for (int j = 0; j < changePerTick; j++)
                {
                    var entity = entities[random.Next(entities.Length)];
                    PositionComponent pos = entity.GetPos();
                    pos.Pos = ((float X, float Y)) (pos.X + random.NextDouble() * 5, pos.Y + random.NextDouble() * 5);
                }

                stopwatch.Start();
                Synchronizer.Update(i);
                stopwatch.Stop();
            }

            Output.WriteLine($"Total {nameof(Synchronizer.Update)} time cost is {stopwatch.ElapsedMilliseconds} ms.");
            Output.WriteLine(
                $"Average {nameof(Synchronizer.Update)} time cost is {stopwatch.ElapsedMilliseconds / (double) testNum} ms.");
            Output.WriteLine($"Total {nameof(entityChangeCount)} count is {entityChangeCount}.");
            Output.WriteLine($"Average {nameof(entityChangeCount)} count is {entityChangeCount / (double) testNum}.");
            Output.WriteLine("Memory Usage: {0}", GC.GetTotalMemory(false));
        }

        [Fact]
        public void PartialViewSyncTest()
        {
            int callbackCount = 0;

            Entity npc = Entities.CreateEntity()
                .Add<PositionComponent>();
            Entity player = Entities.CreateEntity();

            void SyncCallback(ViewChange obj)
            {
                callbackCount++;
                Output.WriteLine($"{nameof(obj.TickCount)} {obj.TickCount}");
                Output.WriteLine(obj.ToString());

                if (obj.EntityChange != null)
                    foreach (EntityDto entityDto in obj.EntityChange)
                    {
                        Output.WriteLine(entityDto.ToString());
                    }

                Output.WriteLine("npc:{0}", npc.GetPos());
                Output.WriteLine("player:{0}", player.GetPos());

                switch (obj.TickCount)
                {
                    case 0:

                        break;
                    case 1:
                        Assert.Equal(0, obj.EntityChange?.Length);
                        Assert.Equal(1, obj.Events?.Length);

                        break;
                    case 2:
                        Assert.Equal(1, obj.EntityChange?.Length);
                        Assert.Equal(0, obj.Events?.Length);

                        break;
                    default:
                        Assert.False(true);

                        break;
                }
            }

            player.Add<PositionComponent>()
                .Add<ObserverComponent>(
                    c =>
                    {
                        c.NotityRange = 5;
                        c.Callback = SyncCallback;
                    });

            foreach (Entity entity in Entities.All.Entities)
            {
                Output.WriteLine(entity.ToString());
            }

            Assert.Equal(2, Entities.All.Entities.Count);

            Synchronizer.Update(0);

            npc.Get<PositionComponent>().Pos = (8, 0);
            Synchronizer.Update(1);

            player.Get<PositionComponent>().Pos = (4, 4);
            Synchronizer.Update(2);

            Assert.Equal(3, callbackCount);
        }
    }
}