using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using Newtonsoft.Json;

using SimCivil.Model;

using static SimCivil.Config;

namespace SimCivil.Map
{
    /// <summary>
    /// Represent a block in map.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// Tile's height.
        /// Defalut sea level is  SimCivil.Config.SeaLevel .
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        [CanBeNull]
        public List<Guid> Items { get; set; }

        /// <summary>
        /// Tile's surface texture.
        /// </summary>
        public string Surface { get; set; }

        /// <summary>
        /// Extra data about Tile.
        /// </summary>
        public dynamic Meta { get; set; }

        /// <summary>
        /// Tile's Position.
        /// </summary>
        public (int X, int Y) Position { get; set; }

        /// <summary>
        /// Method to craete a new Tile
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="surface"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Tile Create((int X, int Y) pos, string surface = "", int height = SeaLevel)
        {
            var e = new Tile
            {
                Height = height,
                Meta = new NullableDynamicObject(),
                Position = pos,
                Surface = surface,
            };
            return e;
        }
    }
}