using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Store
{
    public class ConfigRepository : IPersistable
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ConfigRepository));
        public void Initialize(GameInfo info)
        {
            Random r = new Random(info.Seed);
            Config.Cfg = new Config()
            {
                Name = info.Name,
                Seed = info.Seed,
                SpawnPoint = (
                    r.Next(int.MinValue / 2, r.Next(int.MaxValue / 2)),
                    r.Next(int.MinValue / 2, r.Next(int.MaxValue / 2))
                )
            };
            logger.Info($"Init {nameof(ConfigRepository)}:{info.Name}(SW: {Config.Cfg.SpawnPoint})");
        }

        public void Load(string path)
        {
            string fullPath = Path.Combine(path, Config.DefaultGameConfigFile);
            string json = File.ReadAllText(fullPath);
            Config.Cfg = JsonConvert.DeserializeObject<Config>(json);
            logger.Info($"Loaded {nameof(ConfigRepository)}:{fullPath}");
        }

        public async Task LoadAsync(string path)
        {
            string fullPath = Path.Combine(path, Config.DefaultGameConfigFile);
            string json = await File.ReadAllTextAsync(fullPath);
            Config.Cfg = JsonConvert.DeserializeObject<Config>(json);
            logger.Info($"Loaded {nameof(ConfigRepository)}:{fullPath}");
        }

        public void Save(string path)
        {
            string fullPath = Path.Combine(path, Config.DefaultGameConfigFile);
            string json = JsonConvert.SerializeObject(Config.Cfg);
            File.WriteAllText(fullPath, json);
            logger.Info($"Saved {nameof(ConfigRepository)}:{fullPath}");
        }

        public async Task SaveAsync(string path)
        {
            string fullPath = Path.Combine(path, Config.DefaultGameConfigFile);
            string json = JsonConvert.SerializeObject(Config.Cfg);
            await File.WriteAllTextAsync(fullPath, json);
            logger.Info($"Saved {nameof(ConfigRepository)}:{fullPath}");
        }
    }
}
