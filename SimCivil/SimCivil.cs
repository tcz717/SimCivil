using Autofac;
using log4net;
using SimCivil.Map;
using SimCivil.Net;
using SimCivil.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static SimCivil.Config;

namespace SimCivil
{
    /// <summary>
    /// SimCivil's main logic core.
    /// </summary>
    public class SimCivil
    {
        /// <summary>
        /// SimCivil's logger.
        /// </summary>
        public static readonly ILog logger = LogManager.GetLogger(typeof(SimCivil));
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
        public void Initialize(GameInfo info) => Services.CallMany<IPersistable>(n => n.Initialize(info));
        /// <summary>
        /// Load a game.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        public void Load(string path) => Services.CallMany<IPersistable>(n => n.Load(path));
        /// <summary>
        /// Save a game.
        /// </summary>
        public void Save() => Services.CallMany<IPersistable>(n => n.Save(Info.StoreDirectory));

        /// <summary>
        /// Start game server.
        /// <paramref name="period">Span between ticks.</paramref>
        /// </summary>
        public void Run(int period = DefalutPeriod)
        {
            logger.Info("SimCivil server start running.");
            var tickers = Services.Resolve<IEnumerable<ITicker>>();
            int tickCount = 0;
            while(true)
            {
                var startTime = DateTime.Now;
                foreach (var ticker in tickers)
                {
                    ticker.Update(tickCount);
                }
                var remain = (DateTime.Now - startTime).Milliseconds;
                if (remain > 0)
                    Thread.Sleep(remain);
            };
            logger.Info("SimCivil server stop.");
        }
    }
}
