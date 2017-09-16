using System;

namespace SimCivil.Profile
{
    /// <summary>
    /// Extra information about environment
    /// </summary>
    public class EnvironmentExtra
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentExtra"/> class.
        /// </summary>
        public EnvironmentExtra()
        {
            MachineName = Environment.MachineName;
            Version = Environment.Version.ToString();
            OSVersion = Environment.OSVersion.ToString();
            try
            {
                CommandLineArgs = Environment.GetCommandLineArgs();
            }
            catch (NotSupportedException)
            {
            } // The system does not support command-line arguments.
        }


        /// <summary>
        /// Gets the name of the machine.
        /// </summary>
        /// <value>
        /// The name of the machine.
        /// </value>
        public string MachineName { get; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; }

        /// <summary>
        /// Gets the os version.
        /// </summary>
        /// <value>
        /// The os version.
        /// </value>
        // ReSharper disable once InconsistentNaming
        public string OSVersion { get; }

        /// <summary>
        /// Gets the command line arguments.
        /// </summary>
        /// <value>
        /// The command line arguments.
        /// </value>
        public string[] CommandLineArgs { get; }
    }
}