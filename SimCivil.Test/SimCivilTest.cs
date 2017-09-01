using Autofac;
using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using SimCivil.Map;
using SimCivil.Net;
using SimCivil.Store;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SimCivil.Test
{
    public class SimCivilTest
    {
        [Fact]
        public void ContainerTest()
        {
            SimCivil game = new SimCivil();

            var first = game.Services.Resolve<IServerListener>();
            Assert.NotNull(first);

            var second = game.Services.Resolve<IServerListener>();
            Assert.NotNull(second);

            Assert.Equal(first, second);
        }

        [Fact]
        public void InjectTest()
        {
            using (var service = new AutoFake())
            {
                var map = service.Resolve<MapData>();
                Assert.IsType<MapData>(map);
            }

            var builder = new ContainerBuilder();
            builder.Register(n => A.Fake<IMapGenerator>());
            builder.Register(n => A.Fake<IMapRepository>());
            builder.Register(n => A.Fake<IServerListener>());
            builder.RegisterType<MapData>().SingleInstance();

            using (var Services = builder.Build())
            {
                var map = Services.Resolve<MapData>();
                Assert.IsType<MapData>(map);
            }
        }
    }
}
