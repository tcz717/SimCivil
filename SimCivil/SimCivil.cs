using Autofac;
using log4net;
using SimCivil.Map;
using SimCivil.Net;
using SimCivil.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using static SimCivil.Config;

namespace SimCivil
{
    /// <summary>
    /// The main logic core of SimCivil.
    /// </summary>
    public class SimCivil
    {
        /// <summary>
        /// The logger of SimCivil .
        /// </summary>
        public static readonly ILog Logger = LogManager.GetLogger(typeof(SimCivil));

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
            builder.RegisterType<MatrixServer>()
                .WithParameter("ip", IPAddress.Loopback.ToString())
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
                Logger.Info(
                    $"Service {s.Activator.LimitType} registered as {string.Join(',', s.Services.Select(n => n.Description))}");
            }
        }

        /// <summary>
        /// A container used for dependencies injecting.
        /// </summary>
        public IContainer Services { get; }

        /// <summary>
        /// Basic game information.
        /// </summary>
        public GameInfo Info { get; private set; }

        /// <summary>
        /// Initialize a new game.
        /// </summary>
        /// <param name="info">Game's information.</param>
        public void Initialize(GameInfo info)
        {
            Info = info;
            Logger.Info($"Initialize Game: {info.Name} ({info.StoreDirectory} {info.Seed:X})");
            Directory.CreateDirectory(info.StoreDirectory);
            Services.CallMany<IPersistable>(n => n.Initialize(info));
        }

        /// <summary>
        /// Load a game.
        /// </summary>
        /// <param name="info">Game's information.</param>
        public void Load(GameInfo info)
        {
            Info = info;
            Logger.Info($"Load Game in: {info.StoreDirectory}");
            Services.CallMany<IPersistable>(n => n.Load(info.StoreDirectory));
        }

        /// <summary>
        /// Save a game.
        /// </summary>
        public void Save()
        {
            Logger.Info($"Save Game in: {Info.StoreDirectory}");
            Services.CallMany<IPersistable>(n => n.Save(Info.StoreDirectory));
        }

        /// <summary>
        /// Start game server.
        /// <paramref name="period">Span between ticks.</paramref>
        /// </summary>
        public void Run(int period = DefaultPeriod, bool block = true)
        {
            Logger.Info("SimCivil loop start running.");

            var token = new CancellationTokenSource();
            Task.Factory.StartNew(() => GameLoop(period, token.Token),
                    token.Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default)
                // ReSharper disable once MethodSupportsCancellation
                .ContinueWith(t => Logger.Info("SimCivil stop loop."));

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (block)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Escape:
                        token.Cancel();
                        Logger.Info("keyboard exit requested");
                        return;
                }
            }
        }

        private void GameLoop(int period, CancellationToken token)
        {
            Thread.CurrentThread.Name = "GameLoop";
            //Get tickers
            var tickers = Services.Resolve<IEnumerable<ITicker>>().OrderByDescending(t => t.Priority).ToList();
            int tickCount = 0;
            Map = Services.Resolve<MapData>();

            foreach (var ticker in tickers)
            {
                ticker.Start();
                Logger.Info($"Started Ticker {ticker}");
            }

            while (!token.IsCancellationRequested)
            {
                var startTime = DateTime.Now;
                foreach (var ticker in tickers)
                {
                    ticker.Update(tickCount);
                }
                var duration = (DateTime.Now - startTime).Milliseconds;
                Logger.Debug($"Tick {tickCount} takes {duration} ms.");
                if (duration < period)
                    Thread.Sleep(period - duration);
                else
                    Logger.Warn($"Tick {tickCount} timeout.");
                tickCount++;
            }
            Save();
            //Stop tickers
            foreach (var ticker in tickers)
            {
                ticker.Stop();
                Logger.Info($"Stopped Ticker {ticker}");
            }
        }
    }
}