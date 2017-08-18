using Newtonsoft.Json;
using static SimCivil.Config;

namespace SimCivil.Map
{
    public class Atlas
    {
        public Tile[,] Tiles { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        [JsonIgnore]
        public int Left { get { return X; } }
        [JsonIgnore]
        public int Right { get { return X + Width; } }
        [JsonIgnore]
        public int Top { get { return Y; } }
        [JsonIgnore]
        public int Bottom { get { return Y + Height; } }

        public static Atlas Create(int x, int y, int width = DefaultAtlasWidth, int height = DefaultAtlasHeight)
        {
            return new Atlas()
            {
                Tiles = new Tile[width, height],
                X = x,
                Y = y,
                Width = width,
                Height = height,
            };
        }
    }
}