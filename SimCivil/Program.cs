using log4net;
using System;
using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Autofac.Configuration;
using log4net.Core;

using MongoDB.Driver;

using SimCivil.Auth;
using SimCivil.Contract;
using SimCivil.Model;
using SimCivil.Rpc;
using SimCivil.Store;
using SimCivil.Sync;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log.config", Watch = false)]

namespace SimCivil
{
    internal class Program
    {
        public static readonly ILog Logger = LogManager.GetLogger(Assembly.GetExecutingAssembly(), "Global");

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            GameInfo info = new GameInfo();
            var parser = new CommandLineParser.CommandLineParser();

            try
            {
                parser.ExtractArgumentAttributes(info);
                parser.ParseCommandLine(args);
            }
            catch (CommandLineParser.Exceptions.CommandLineException e)
            {
                Console.WriteLine(e.Message);
                parser.ShowUsage();
            }

            if (parser.ParsingSucceeded)
            {
                if (info.Seed == 0)
                {
                    Random r = new Random();
                    info.Seed = r.Next(int.MinValue, int.MaxValue);
                    Logger.Info($"Using auto seed {info.Seed}");
                }
                SimCivil game = !string.IsNullOrWhiteSpace(info.Config)
                    ? new SimCivil(LoadConfiguration(info.Config))
                    : new SimCivil(LoadConfiguration());
                if (info.IsCreate)
                    game.Initialize(info);
                else
                    game.Load(info);

                ((log4net.Repository.Hierarchy.Hierarchy) LogManager.GetRepository(Assembly.GetEntryAssembly()))
                    .Threshold = info.Visibility ? Level.Debug : Level.Info;
                game.Run();
            }
            Console.Read();
        }

        private static IContainer LoadConfiguration(string config = "configuration.json")
        {
            var builder = new ContainerBuilder();
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(config);
            var module = new ConfigurationModule(configBuilder.Build());

            builder.RegisterModule(module);
            builder.UseRpcSession();
            builder.RegisterRpcProvider<RoleManager, IRoleManager>().InstancePerChannel();
            builder.RegisterRpcProvider<ChunkViewSynchronizer, IViewSynchronizer>().SingleInstance();
            builder.RegisterRpcProvider<LocalEntityManager, IEntityManager>().SingleInstance();
            builder.RegisterType<MongoDbPlayerRepo>().AsImplementedInterfaces();
            builder.RegisterInstance(new MongoClient().GetDatabase(nameof(SimCivil)));

            return builder.Build();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Fatal("UnhandledException!", e.ExceptionObject as Exception);
        }
    }
}