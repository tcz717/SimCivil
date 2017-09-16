using Newtonsoft.Json;
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
        /// Five slots:
        /// <list type="bullet">
        /// <item>TopLeft</item>
        /// <item>TopRight</item>
        /// <item>BottomLeft</item>
        /// <item>BottomRight</item>
        /// <item>Centor</item>
        /// </list>
        /// </summary>
        public Slot[] Slots { get; set; }
        /// <summary>
        /// Four sides:
        /// <list type="bullet">
        /// <item>Top</item>
        /// <item>Bottom</item>
        /// <item>Left</item>
        /// <item>Right</item>
        /// </list>
        /// </summary>
        public Side[] Sides { get; set; }
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
                Slots = new Slot[5],
                Sides = new Side[4],
            };
            return e;
        }
    }
}