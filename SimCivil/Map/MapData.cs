using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Map
{
    public class MapData
    {
        public Dictionary<(int X, int Y), Atlas> AtlasCollection { get; private set; }

        public Tile this[int x, int y]
        {
            get
            {
                foreach (var atlas in AtlasCollection.Values)
                {
                    if (x >= atlas.Left && x >= atlas.Top &&
                        x < atlas.Right && x < atlas.Bottom)
                    {
                        return atlas.Tiles[x, x];
                    }
                }
                throw new IndexOutOfRangeException();
            }
        }

        public Tile this[(int X, int Y) pos] { get => this[pos.X, pos.Y]; }
    }
}
