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
        public static Config Cfg { get; set; }

        public const int DefaultPort = 20170;
        public const int DefalutPeriod = 50;

        public const int DefaultPingRequestSecond = 30;
        public const int DefaultLostConnectionSecond = 60;

        public static readonly Version SimCivilProtocolVersion = new Version(1, 1);

        public static readonly TimeSpan PingRequestTime = TimeSpan.FromSeconds(DefaultPingRequestSecond);
        public static readonly TimeSpan LostConnectionTime = TimeSpan.FromSeconds(DefaultLostConnectionSecond);

        public const int DefaultAtlasWidth = 64;
        public const int DefaultAtlasHeight = 64;
        public const int MaxGroundEntities = 4;
        public const int SeaLevel = 64;

        public const string DefaultGameConfigFile = "game.json";
        public const string DefaultConfigFile = "configuration.json";

        public int PlantHeightLimit { get; set; } = 200;
        public double PlantDensity { get; set; } = 0.3;

        public (int X, int Y) SpawnPoint { get; set; }
        public int Seed { get; set; }
        public string Name { get; set; }

        static Config()
        {
            Cfg = new Config();
        }
    }
}