using SimCivil.Map;

namespace SimCivil.Store
{
    /// <summary>
    /// Repository used to store map data.
    /// </summary>
    public interface IMapRepository
    {
        /// <summary>
        /// Check if one Atlas has been stored.
        /// </summary>
        /// <param name="atlasIndex"></param>
        /// <returns>Cheak result.</returns>
        bool Contains((int X, int Y) atlasIndex);

        /// <summary>
        /// Get one atlas from repository.
        /// </summary>
        /// <param name="atlasIndex">Atlas' position.</param>
        /// <returns>Wanted Atlas</returns>
        Atlas GetAtlas((int X, int Y) atlasIndex);

        /// <summary>
        /// Add one Atlas to track. And will be stored when nessesary.
        /// </summary>
        /// <param name="atlasIndex">Atlas' position.</param>
        /// <param name="atlas">Atlas that will be tracked.</param>
        void PutAtlas((int X, int Y) atlasIndex, Atlas atlas);
    }
}