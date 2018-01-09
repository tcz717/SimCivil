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
// SimCivil - SimCivil - JsonEntityRepo.cs
// Create Date: 2018/01/07
// Update Date: 2018/01/08

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;

using Newtonsoft.Json;

using SimCivil.Model;

using static SimCivil.Config;

namespace SimCivil.Store
{
    /// <summary>
    /// Json Entity Repository
    /// </summary>
    /// <seealso>
    ///     <cref>SimCivil.Store.MemoryEntityRepo</cref>
    /// </seealso>
    /// <seealso>
    ///     <cref>SimCivil.Store.IPrefabRepository</cref>
    /// </seealso>
    /// <seealso>
    ///     <cref>SimCivil.Store.IPersistable</cref>
    /// </seealso>
    public class JsonEntityRepo : MemoryEntityRepo, IPrefabRepository, IPersistable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(JsonMapRepo));

        /// <summary>
        /// Gets or sets the root path.
        /// </summary>
        /// <value>
        /// The root path.
        /// </value>
        public string RootPath { get; set; }

        /// <summary>
        /// Gets or sets the prefabs.
        /// </summary>
        /// <value>
        /// The prefabs.
        /// </value>
        public Dictionary<string, Entity> Prefabs { get; set; }

        /// <summary>
        /// Initialize the store.
        /// </summary>
        /// <param name="info"></param>
        public void Initialize(GameInfo info)
        {
            Prefabs = new Dictionary<string, Entity>();
            Entities = new Dictionary<Guid, Entity>();
            RootPath = info.StoreDirectory;
        }

        /// <summary>
        /// Load the object.
        /// </summary>t
        /// <param name="path">Directory path to store all data.</param>
        public void Load(string path)
        {
            LoadAsync(path).Wait();
        }

        /// <summary>
        /// Load the object async.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        public async Task LoadAsync(string path)
        {
            var fullPath = Path.Combine(path, $"{nameof(JsonEntityRepo)}.json");
            RootPath = path;
            var entities = JsonConvert.DeserializeObject<IList<Entity>>(await File.ReadAllTextAsync(fullPath));
            Logger.Info($"Loaded {entities.Count} entities in {fullPath}.");

            Entities = entities.ToDictionary(e => e.Id);
            Prefabs = entities.Where(e => e.Meta.ContainsKey(PrefabNameKey))
                .ToDictionary(e => e.Meta[PrefabNameKey].ToString());
            Logger.Info($"Found {Prefabs.Count} prefabs.");
        }

        /// <summary>
        /// Save the object.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        public void Save(string path)
        {
            SaveAsync(path).Wait();
        }

        /// <summary>
        /// Save the object async.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        public async Task SaveAsync(string path)
        {
            var fullPath = Path.Combine(path, $"{nameof(JsonEntityRepo)}.json");
            await File.WriteAllTextAsync(fullPath, JsonConvert.SerializeObject(Entities.Values));
            Logger.Info($"Saved {Entities.Count} entities in {fullPath}.");
        }

        /// <summary>
        /// Gets the prefab.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        public Entity GetPrefab(string tag)
        {
            return Prefabs[tag];
        }

        /// <summary>
        /// Sets the prefab.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="prefab">The prefab.</param>
        public void SetPrefab(string tag, Entity prefab)
        {
            prefab.Meta[PrefabNameKey] = tag;
            Prefabs[tag] = prefab;
            Entities[prefab.Id] = prefab;
        }
    }
}