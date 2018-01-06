using Autofac;
using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using SimCivil.Map;
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
            builder.RegisterType<MapData>().SingleInstance();

            using (var services = builder.Build())
            {
                var map = services.Resolve<MapData>();
                Assert.IsType<MapData>(map);
            }
        }
    }
}
