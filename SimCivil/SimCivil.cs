using Autofac;
using SimCivil.Map;
using SimCivil.Net;
using SimCivil.Store;
using System.Collections.Generic;
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

            Services = builder.Build();
        }

        /// <summary>
        /// Create game from specific config.
        /// </summary>
        /// <param name="container"></param>
        public SimCivil(IContainer container)
        {
            Services = container;
        }

        /// <summary>
        /// A container used for denpendencies injecting.
        /// </summary>
        public IContainer Services { get; private set; }
        /// <summary>
        /// Basic game infomation.
        /// </summary>
        public GameInfo Info { get; private set; }

        /// <summary>
        /// Initialize a new game.
        /// </summary>
        /// <param name="info">Game's infomation.</param>
        public void Initialize(GameInfo info)
        {
            Info = info;
            var persistableSeriveces = Services.Resolve<IEnumerable<IPersistable>>();
            foreach (var service in persistableSeriveces)
            {
                service.Initialize(info);
            }
        }
        /// <summary>
        /// Load a game.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        public void Load(string path)
        {
            var persistableSeriveces = Services.Resolve<IEnumerable<IPersistable>>();
            foreach (var service in persistableSeriveces)
            {
                service.Load(path);
            }
        }
        /// <summary>
        /// Save a game.
        /// </summary>
        public void Save()
        {
            var persistableSeriveces = Services.Resolve<IEnumerable<IPersistable>>();
            foreach (var service in persistableSeriveces)
            {
                service.Save(Info.StoreDirectory);
            }
        }

        /// <summary>
        /// Start game server.
        /// </summary>
        public void Run()
        {

        }
    }
}
