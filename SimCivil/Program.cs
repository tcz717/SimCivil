using log4net;
using System;
using System.Reflection;

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
                if (info.IsCreate)
                    game.Initialize(info);
                else
                    game.Load(info);
                game.Run();
            }
            Console.Read();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Fatal("UnhandledException!", e.ExceptionObject as Exception);
        }
    }
}