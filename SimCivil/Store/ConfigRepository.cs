using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Store
{
    /// <inheritdoc />
    /// <summary>
    /// Repository stored config
    /// </summary>
    /// <seealso>
    ///     <cref>SimCivil.Store.IPersistable</cref>
    /// </seealso>
    public class ConfigRepository : IPersistable
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ConfigRepository));
        /// <inheritdoc />
        /// <summary>
        /// Initialize the store.
        /// </summary>
        /// <param name="info"></param>
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

        /// <inheritdoc />
        /// <summary>
        /// Load the config.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        public void Load(string path)
        {
            string fullPath = Path.Combine(path, Config.DefaultGameConfigFile);
            string json = File.ReadAllText(fullPath);
            Config.Cfg = JsonConvert.DeserializeObject<Config>(json);
            logger.Info($"Loaded {nameof(ConfigRepository)}:{fullPath}");
        }

        /// <inheritdoc />
        /// <summary>
        /// Load the config async.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        /// <returns></returns>
        public async Task LoadAsync(string path)
        {
            string fullPath = Path.Combine(path, Config.DefaultGameConfigFile);
            string json = await File.ReadAllTextAsync(fullPath);
            Config.Cfg = JsonConvert.DeserializeObject<Config>(json);
            logger.Info($"Loaded {nameof(ConfigRepository)}:{fullPath}");
        }

        /// <inheritdoc />
        /// <summary>
        /// Save the config.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        public void Save(string path)
        {
            string fullPath = Path.Combine(path, Config.DefaultGameConfigFile);
            string json = JsonConvert.SerializeObject(Config.Cfg);
            File.WriteAllText(fullPath, json);
            logger.Info($"Saved {nameof(ConfigRepository)}:{fullPath}");
        }

        /// <inheritdoc />
        /// <summary>
        /// Save the config async.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        /// <returns></returns>
        public async Task SaveAsync(string path)
        {
            string fullPath = Path.Combine(path, Config.DefaultGameConfigFile);
            string json = JsonConvert.SerializeObject(Config.Cfg);
            await File.WriteAllTextAsync(fullPath, json);
            logger.Info($"Saved {nameof(ConfigRepository)}:{fullPath}");
        }
    }
}
