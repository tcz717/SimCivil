using Autofac;
using SimCivil.Net;
using System;
using System.Collections.Generic;
using System.Text;
using SimCivil.Map;
using static SimCivil.Config;

namespace SimCivil
{
    /// <summary>
    /// SimCivil's main logic core.
    /// </summary>
    public class SimCivil
    {
        /// <summary>
        /// Game's map data.
        /// </summary>
        public MapData Map { get; private set; }
        /// <summary>
        /// Default game config.
        /// </summary>
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

        /// <summary>
        /// Create game from specific config.
        /// </summary>
        /// <param name="container"></param>
        public SimCivil(IContainer container)
        {
            Initialization(container);
        }

        /// <summary>
        /// A container used for denpendencies injecting.
        /// </summary>
        public IContainer Container { get; private set; }

        private void Initialization(IContainer container)
        {
            Container = container;
        }

        /// <summary>
        /// Start game server.
        /// </summary>
        public void Run()
        {

        }
    }
}
