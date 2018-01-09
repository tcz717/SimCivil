using System;
using System.Collections.Generic;

namespace SimCivil
{
    /// <summary>
    /// Save all Configs include constant value and variable value.
    /// It's recommend to put 
    /// <code>
    /// using static SimCivil.Config;
    /// </code>
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Gets or sets the Config.
        /// </summary>
        /// <value>
        /// The Config.
        /// </value>
        public static Config Cfg { get; set; }

        /// <summary>
        /// The default port
        /// </summary>
        public const int DefaultPort = 20170;


        /// <summary>
        /// The default period
        /// </summary>
        public const int DefaultPeriod = 50;

        /// <summary>
        /// The default ping request second
        /// </summary>
        public const int DefaultPingRequestSecond = 30;

        /// <summary>
        /// The default lost connection second
        /// </summary>
        public const int DefaultLostConnectionSecond = 60;

        /// <summary>
        /// The sim civil protocol version
        /// </summary>
        public static readonly Version SimCivilProtocolVersion = new Version(1, 0);

        /// <summary>
        /// The ping request time
        /// </summary>
        public static readonly TimeSpan PingRequestTime = TimeSpan.FromSeconds(DefaultPingRequestSecond);

        /// <summary>
        /// The lost connection time
        /// </summary>
        public static readonly TimeSpan LostConnectionTime = TimeSpan.FromSeconds(DefaultLostConnectionSecond);

        /// <summary>
        /// The default atlas width
        /// </summary>
        public const int DefaultAtlasWidth = 64;

        /// <summary>
        /// The default atlas height
        /// </summary>
        public const int DefaultAtlasHeight = 64;

        /// <summary>
        /// The maximum ground entities
        /// </summary>
        public const int MaxGroundEntities = 4;

        /// <summary>
        /// The sea level
        /// </summary>
        public const int SeaLevel = 64;

        /// <summary>
        /// The default game configuration file
        /// </summary>
        public const string DefaultGameConfigFile = "game.json";

        /// <summary>
        /// The default configuration file
        /// </summary>
        public const string DefaultConfigFile = "configuration.json";

        /// <summary>
        /// The prefab name key
        /// </summary>
        public const string PrefabNameKey = "PrefabName";

        /// <summary>
        /// Gets or sets the plant height limit.
        /// </summary>
        /// <value>
        /// The plant height limit.
        /// </value>
        public int PlantHeightLimit { get; set; } = 200;

        /// <summary>
        /// Gets or sets the plant density.
        /// </summary>
        /// <value>
        /// The plant density.
        /// </value>
        public double PlantDensity { get; set; } = 0.3;

        /// <summary>
        /// Gets or sets the spawn point.
        /// </summary>
        /// <value>
        /// The spawn point.
        /// </value>
        public (int X, int Y) SpawnPoint { get; set; }

        /// <summary>
        /// Gets or sets the seed.
        /// </summary>
        /// <value>
        /// The seed.
        /// </value>
        public int Seed { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the available races.
        /// </summary>
        /// <value>
        /// The available races.
        /// </value>
        public string[] AvailableRaces { get; set; } = {"human"};

        /// <summary>
        /// The available sex
        /// </summary>
        public static readonly string[] AvailableSex = {"male", "female"};

        static Config()
        {
            Cfg = new Config();
        }
    }
}