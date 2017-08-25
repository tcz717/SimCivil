using System;
using static SimCivil.Config;

namespace SimCivil.Map
{
    /// <summary>
    /// Interface supporting generating map.
    /// </summary>
    public interface IMapGenerator
    {
        /// <summary>
        /// Generate new Atlas
        /// </summary>
        /// <param name="x">Index of the atlas</param>
        /// <param name="y">Index of the atlas</param>
        /// <param name="width">Atlas's Width</param>
        /// <param name="height">Atlas's Height</param>
        /// <returns></returns>
        Atlas Generate(int x, int y, int width = DefaultAtlasWidth, int height = DefaultAtlasHeight);

        /// <summary>
        /// Event trigered on generating new tile.
        /// </summary>
        event GeneratingEventHandler OnGenerating;
    }

    public delegate void GeneratingEventHandler(IMapGenerator sender, GeneratingEventArgs args);

    public class GeneratingEventArgs : EventArgs
    {
        public (int X, int Y) Position { get; set; }
        public Tile Tile { get; set; }
        public GeneratingEventArgs((int X, int Y) position, Tile tile)
        {
            Position = position;
            Tile = tile;
        }
    }
}