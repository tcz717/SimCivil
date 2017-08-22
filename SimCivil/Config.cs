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

        public const int DefaultAtlasWidth = 256;
        public const int DefaultAtlasHeight = 256;
        public const int MaxGroundEntities = 4;
        public const int SeaLevel = 64;
        
        public int PlantHeightLimit { get; set; } = 200;
        public double PlantDensity { get; set; } = 0.3;

        static Config()
        {
            Cfg = new Config();
        }
    }
}