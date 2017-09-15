using System;

namespace SimCivil.Profile
{
    public class EnvironmentExtra
    {
        public EnvironmentExtra()
        {
            MachineName = Environment.MachineName;
            Version = Environment.Version.ToString();
            OSVersion = Environment.OSVersion.ToString();
            try
            {
                CommandLineArgs = Environment.GetCommandLineArgs();
            }
            catch (NotSupportedException) { } // The system does not support command-line arguments.
        }


        public string MachineName { get; }

        public string Version { get; }

        public string OSVersion { get; }

        public string[] CommandLineArgs { get; }
    }
}