using Autofac;
using SimCivil.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil
{
    public class SimCivil
    {
        public SimCivil()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ServerListener>()
                .WithParameter("port", 20170)
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
