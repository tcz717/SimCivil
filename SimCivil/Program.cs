﻿using log4net;
using System;
using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Autofac.Configuration;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log.config", Watch = false)]

namespace SimCivil
{
    class Program
    {
        public static readonly ILog logger = LogManager.GetLogger(Assembly.GetExecutingAssembly(), "Global");
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            GameInfo info = new GameInfo();
            SimCivil game = new SimCivil();
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

            if(parser.ParsingSucceeded)
            {
                if (info.Seed == 0)
                {
                    Random r = new Random();
                    info.Seed = r.Next(int.MinValue, int.MaxValue);
                    logger.Info($"Using auto seed {info.Seed}");
                }
                if (!string.IsNullOrWhiteSpace(info.Config))
                    game = new SimCivil(LoadConfiguration(info.Config));
                else
                    game = new SimCivil(LoadConfiguration(Config.DefaultConfigFile));
                if (info.IsCreate)
                    game.Initialize(info);
                else
                    game.Load(info);
                game.Run();
            }
            Console.Read();
        }

        private static IContainer LoadConfiguration(string config)
        {
            var builder = new ContainerBuilder();
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("configuration.json");
            var module = new ConfigurationModule(configBuilder.Build());
            
            builder.RegisterModule(module);

            return builder.Build();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Fatal("UnhandledException!", e.ExceptionObject as Exception);
        }
    }
}