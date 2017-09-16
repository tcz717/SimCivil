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

    /// <summary>
    /// When a new atlas is generating.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="GeneratingEventArgs"/> instance containing the event data.</param>
    public delegate void GeneratingEventHandler(IMapGenerator sender, GeneratingEventArgs args);

    /// <summary>
    /// Represent a new atlas generating.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class GeneratingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public (int X, int Y) Position { get; set; }
        /// <summary>
        /// Gets or sets the tile.
        /// </summary>
        /// <value>
        /// The tile.
        /// </value>
        public Tile Tile { get; set; }
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SimCivil.Map.GeneratingEventArgs" /> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="tile">The tile.</param>
        public GeneratingEventArgs((int X, int Y) position, Tile tile)
        {
            Position = position;
            Tile = tile;
        }
    }
}