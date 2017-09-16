using System;
using System.Collections.Generic;
using System.Text;
using SimCivil.Map;
using Newtonsoft.Json;
using System.IO;
using log4net;
using System.Threading.Tasks;
using System.Linq;

namespace SimCivil.Store
{
    /// <summary>
    /// Save atlas in json file.
    /// </summary>
    public class JsonMapRepo : MemoryMapRepo, IPersistable
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(JsonMapRepo));

        /// <summary>
        /// Gets the root path.
        /// </summary>
        /// <value>
        /// The root path.
        /// </value>
        public string RootPath { get; private set; }
        /// <summary>
        /// Gets the index of the atlas.
        /// </summary>
        /// <value>
        /// The index of the atlas.
        /// </value>
        public HashSet<(int X, int Y)> AtlasIndex { get; private set; }

        /// <summary>
        /// Initialize the store.
        /// </summary>
        /// <param name="info"></param>
        public void Initialize(GameInfo info)
        {
            AtlasIndex = new HashSet<(int X, int Y)>();
            RootPath = info.StoreDirectory;
        }

        /// <summary>
        /// Load the object.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        public void Load(string path)
        {
            var fullPath = Path.Combine(path, $"{nameof(AtlasIndex)}.json");
            RootPath = path;
            AtlasIndex = JsonConvert.DeserializeObject<HashSet<(int X, int Y)>>(File.ReadAllText(fullPath));
            logger.Info($"Loaded atlas index file in {fullPath}.");
        }

        /// <summary>
        /// Save the object.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        public void Save(string path)
        {
            var fullPath = Path.Combine(path, $"{nameof(AtlasIndex)}.json");
            File.WriteAllText(fullPath, JsonConvert.SerializeObject(AtlasIndex));
            logger.Info($"Saved atlas index file in {fullPath}.");

            foreach (var key in AtlasStore.Keys)
            {
                fullPath = Path.Combine(path, $"{key.X}_{key.Y}.json");
                File.WriteAllText(fullPath, JsonConvert.SerializeObject(AtlasStore[key]));
                logger.Info($"Saved ${key} atlas file in {fullPath}.");
            }
        }
        /// <summary>
        /// Gets the atlas.
        /// </summary>
        /// <param name="atlasIndex">Index of the atlas.</param>
        /// <returns></returns>
        public override Atlas GetAtlas((int X, int Y) atlasIndex)
        {
            if (base.Contains(atlasIndex))
                return base.GetAtlas(atlasIndex);
            else
            {
                Atlas atlas = LoadAtlas(atlasIndex);
                PutAtlas(atlasIndex, atlas);
                return atlas;
            }
        }

        private Atlas LoadAtlas((int X, int Y) atlasIndex)
        {
            var fullPath = Path.Combine(RootPath, $"{atlasIndex.X}_{atlasIndex.Y}.json");
            Atlas atlas = JsonConvert.DeserializeObject<Atlas>(File.ReadAllText(fullPath));
            logger.Info($"Loaded ${atlasIndex} atlas file from {fullPath}.");
            return atlas;
        }

        /// <summary>
        /// Load the object async.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        /// <returns></returns>
        public async Task LoadAsync(string path)
        {
            var fullPath = Path.Combine(path, $"{nameof(AtlasIndex)}.json");
            RootPath = path;
            AtlasIndex = JsonConvert.DeserializeObject<HashSet<(int X, int Y)>>(await File.ReadAllTextAsync(fullPath));
            logger.Info($"Loaded atlas index file in {fullPath}.");
        }

        /// <summary>
        /// Save the object async.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        /// <returns></returns>
        public async Task SaveAsync(string path)
        {
            var fullPath = Path.Combine(path, $"{nameof(AtlasIndex)}.json");
            await File.WriteAllTextAsync(fullPath, JsonConvert.SerializeObject(AtlasIndex));
            logger.Info($"Saved atlas index file in {fullPath}.");

            var writeTasks = AtlasStore.Select(async a =>
            {
                fullPath = Path.Combine(path, $"{a.Key.X}_{a.Key.Y}.json");
                await File.WriteAllTextAsync(fullPath, JsonConvert.SerializeObject(a.Value));
                logger.Info($"Saved ${a.Key} atlas file in {fullPath}.");
            });
            await Task.WhenAll(writeTasks);
        }
    }
}
