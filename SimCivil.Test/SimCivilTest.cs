using Autofac;
using SimCivil.Net;
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

            var first = game.Container.Resolve<IServerListener>();
            Assert.NotNull(first);

            var second = game.Container.Resolve<IServerListener>();
            Assert.NotNull(second);

            Assert.Equal(first, second);
        }
    }
}
