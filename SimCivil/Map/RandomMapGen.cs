using System;
using System.Collections.Generic;
using System.Text;
using static SimCivil.Config;

namespace SimCivil.Map
{
    /// <summary>
    /// Random map generator used for test.
    /// </summary>
    public class RandomMapGen : IMapGenerator
    {
        private Random rand;

        public event GeneratingEventHandler OnGenerating;

        /// <inheritdoc />
        /// <summary>
        /// Generate new Atlas
        /// </summary>
        /// <param name="x">Index of the atlas</param>
        /// <param name="y">Index of the atlas</param>
        /// <param name="width">Atlas's Width</param>
        /// <param name="height">Atlas's Height</param>
        /// <returns></returns>
        public Atlas Generate(int x, int y, int width = DefaultAtlasWidth, int height = DefaultAtlasHeight)
        {
            Atlas atlas = Atlas.Create(x, y, width, height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    atlas.Tiles[i, j] = Tile.Create((x * width + i, y * height + j),
                        height: rand.Next(SeaLevel - 10, SeaLevel + 10));
                    OnGenerating?.Invoke(this, new GeneratingEventArgs((i, j), atlas.Tiles[i, j]));
                }
            }
            return atlas;
        }

        public RandomMapGen()
        {
            rand = new Random(Cfg.Seed);
        }
    }
}
