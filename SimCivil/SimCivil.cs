using Autofac;
using SimCivil.Net;
using System;
using System.Collections.Generic;
using System.Text;
using SimCivil.Map;
using static SimCivil.Config;

namespace SimCivil
{
    public class SimCivil
    {
        public MapData Map { get; private set; }
        public SimCivil()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ServerListener>()
                .WithParameter("port", DefaultPort)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterInstance(this);

            Initialization(builder.Build());
        }

        public SimCivil(IContainer container)
        {
            Initialization(container);
        }

        public IContainer Container { get; private set; }

        private void Initialization(IContainer container)
        {
            Container = container;
        }

        public void Run()
        {

        }
    }
}
