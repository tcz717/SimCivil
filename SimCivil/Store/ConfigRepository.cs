// Copyright (c) 2017 TPDT
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
// SimCivil - SimCivil - ConfigRepository.cs
// Create Date: 2017/08/24
// Update Date: 2018/02/22

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using log4net;

using Newtonsoft.Json;

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
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConfigRepository));

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
            Logger.Info($"Init {nameof(ConfigRepository)}:{info.Name}(SW: {Config.Cfg.SpawnPoint})");
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
            Logger.Info($"Loaded {nameof(ConfigRepository)}:{fullPath}");
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
            Logger.Info($"Loaded {nameof(ConfigRepository)}:{fullPath}");
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
            Logger.Info($"Saved {nameof(ConfigRepository)}:{fullPath}");
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
            Logger.Info($"Saved {nameof(ConfigRepository)}:{fullPath}");
        }
    }
}