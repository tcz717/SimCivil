using System;
using System.Collections.Generic;
using System.Text;
using SimCivil.Map;

namespace SimCivil.Store
{
    /// <summary>
    /// A map repository storing atlas in memory. Just designed for test.
    /// </summary>
    public class MemoryMapRepo : IMapRepository
    {
        /// <summary>
        /// The atlas store
        /// </summary>
        protected Dictionary<(int X, int Y), Atlas> AtlasStore;

        /// <summary>
        /// Check if one Atlas has been stored.
        /// </summary>
        /// <param name="atlasIndex"></param>
        /// <returns>
        /// Cheak result.
        /// </returns>
        public virtual bool Contains((int X, int Y) atlasIndex) => AtlasStore.ContainsKey(atlasIndex);

        /// <summary>
        /// Get one atlas from repository.
        /// </summary>
        /// <param name="atlasIndex">Atlas' position.</param>
        /// <returns>
        /// Wanted Atlas
        /// </returns>
        public virtual Atlas GetAtlas((int X, int Y) atlasIndex) => AtlasStore[atlasIndex];

        /// <summary>
        /// Add one Atlas to track. And will be stored when nessesary.
        /// </summary>
        /// <param name="atlasIndex">Atlas' position.</param>
        /// <param name="atlas">Atlas that will be tracked.</param>
        public virtual void PutAtlas((int X, int Y) atlasIndex, Atlas atlas) => AtlasStore[atlasIndex] = atlas;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMapRepo"/> class.
        /// </summary>
        public MemoryMapRepo()
        {
            AtlasStore = new Dictionary<(int X, int Y), Atlas>();
        }
    }
}
