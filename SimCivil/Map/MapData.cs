using System;
using System.Collections.Generic;
using System.Text;
using static SimCivil.Config;

namespace SimCivil.Map
{
    public class MapData
    {
        public Dictionary<(int X, int Y), Atlas> AtlasCollection { get; private set; }
        public IMapGenerator MapGenerator { get; private set; }
        public bool AllowExpanding { get; set; } = true;

        public Tile this[int x, int y]
        {
            get
            {
                foreach (var atlas in AtlasCollection.Values)
                {
                    if (x >= atlas.Left && x >= atlas.Top &&
                        x < atlas.Right && x < atlas.Bottom)
                    {
                        return atlas.Tiles[x, y];
                    }
                }

                if(AllowExpanding)
                {
                    var atlasIndex = (X: x % DefaultAtlasWidth, Y: y % DefaultAtlasHeight);
                    var newAtlas = MapGenerator.Generate(atlasIndex.X, atlasIndex.Y);
                    AtlasCollection[atlasIndex] = newAtlas;
                    return newAtlas.Tiles[x, y];

                }
                throw new IndexOutOfRangeException();
            }
        }

        public Tile this[(int X, int Y) pos] { get => this[pos.X, pos.Y]; }

        public MapData(IMapGenerator mapGenerator)
        {
            MapGenerator = mapGenerator;
        }
    }
}
