using Autofac;
using log4net;
using SimCivil.Map;
using SimCivil.Net;
using SimCivil.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

            Services = builder.Build();
        }

        /// <summary>
        /// Create game from specific config.
        /// </summary>
        /// <param name="container"></param>
        public SimCivil(IContainer container)
        {
            Services = container;
            foreach (var s in Services.ComponentRegistry.Registrations)
            {
                logger.Info(
                    $"Service {s.Activator.LimitType} registered as {string.Join(',', s.Services.Select(n => n.Description))}");
            }
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
            logger.Info($"Initialize Game: {info.Name} ({info.StoreDirectory} {info.Seed:X})");
            Directory.CreateDirectory(info.StoreDirectory);
            Services.CallMany<IPersistable>(n => n.Initialize(info));
        }

        /// <summary>
        /// Load a game.
        /// </summary>
        /// <param name="info">Game's infomation.</param>
        public void Load(GameInfo info)
        {
            Info = info;
            logger.Info($"Load Game in: {info.StoreDirectory}");
            Services.CallMany<IPersistable>(n => n.Load(info.StoreDirectory));
        }

        /// <summary>
        /// Save a game.
        /// </summary>
        public void Save()
        {
            logger.Info($"Save Game in: {Info.StoreDirectory}");
            Services.CallMany<IPersistable>(n => n.Save(Info.StoreDirectory));
        }

        /// <summary>
        /// Start game server.
        /// <paramref name="period">Span between ticks.</paramref>
        /// </summary>
        public void Run(int period = DefalutPeriod, bool block = true)
        {
            logger.Info("SimCivil loop start running.");

            var token = new CancellationTokenSource();
            Task.Factory.StartNew(() => GameLoop(period, token.Token),
                    token.Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default)
                // ReSharper disable once MethodSupportsCancellation
                .ContinueWith(t => logger.Info("SimCivil stop loop."));

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (block)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Escape:
                        token.Cancel();
                        logger.Info("keyborad exit requested");
                        return;
                }
            }
        }

        private void GameLoop(int period, CancellationToken token)
        {
            Thread.CurrentThread.Name = "GameLoop";
            //Get tickers
            var tickers = Services.Resolve<IEnumerable<ITicker>>().OrderByDescending(t => t.Priority);
            int tickCount = 0;
            Map = Services.Resolve<MapData>();

            foreach (var ticker in tickers)
            {
                ticker.Start();
                logger.Info($"Started Ticker {ticker}");
            }

            while (!token.IsCancellationRequested)
            {
                var startTime = DateTime.Now;
                foreach (var ticker in tickers)
                {
                    ticker.Update(tickCount);
                }
                var duration = (DateTime.Now - startTime).Milliseconds;
                logger.Debug($"Tick {tickCount} takes {duration} ms.");
                if (duration < period)
                    Thread.Sleep(period - duration);
                else
                    logger.Warn($"Tick {tickCount} timeout.");
                tickCount++;
            }
            ;
            Save();
            //Stop tickers
            foreach (var ticker in tickers)
            {
                ticker.Stop();
                logger.Info($"Stoped Ticker {ticker}");
            }
        }
    }
}