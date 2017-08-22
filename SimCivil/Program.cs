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

            SimCivil game = new SimCivil();
            game.Run();
            Console.Read();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Fatal("UnhandledException!", e.ExceptionObject as Exception);
        }
    }
}