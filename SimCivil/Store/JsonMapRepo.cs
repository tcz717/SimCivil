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
        static readonly ILog logger = LogManager.GetLogger(typeof(JsonMapRepo));

        public string RootPath { get; private set; }
        public HashSet<(int X, int Y)> AtlasIndex { get; private set; }

        public void Initialize(GameInfo info)
        {
            AtlasIndex = new HashSet<(int X, int Y)>();
            RootPath = info.StoreDirectory;
        }

        public void Load(string path)
        {
            var fullPath = Path.Combine(path, $"{nameof(AtlasIndex)}.json");
            RootPath = path;
            AtlasIndex = JsonConvert.DeserializeObject<HashSet<(int X, int Y)>>(File.ReadAllText(fullPath));
            logger.Info($"Loaded atlas index file in {fullPath}.");
        }

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

        public async Task LoadAsync(string path)
        {
            var fullPath = Path.Combine(path, $"{nameof(AtlasIndex)}.json");
            RootPath = path;
            AtlasIndex = JsonConvert.DeserializeObject<HashSet<(int X, int Y)>>(await File.ReadAllTextAsync(fullPath));
            logger.Info($"Loaded atlas index file in {fullPath}.");
        }

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
