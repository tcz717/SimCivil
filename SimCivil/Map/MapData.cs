using SimCivil.Store;
using System;
using System.Collections.Generic;
using System.Text;
using static SimCivil.Config;

namespace SimCivil.Map
{
    /// <summary>
    /// Map consists of Atlas.
    /// </summary>
    public class MapData
    {
        public Dictionary<(int X, int Y), Atlas> AtlasCollection { get; private set; }
        public IMapGenerator MapGenerator { get; private set; }
        public IMapRepository MapRepository { get; }

        /// <summary>
        /// Whether allowing expanding when tile not exsists.
        /// </summary>
        public bool AllowExpanding { get; set; } = true;

        /// <summary>
        /// Get tile by position.
        /// </summary>
        /// <param name="x">Tile's X position</param>
        /// <param name="y">Tile's Y position</param>
        /// <returns>Tile you want.</returns>
        /// <exception cref="IndexOutOfRangeException">Tile isn't exsist and expanding is not allowed.</exception>
        public Tile this[int x, int y]
        {
            get
            {
                var atlasIndex = (X: x % DefaultAtlasWidth, Y: y % DefaultAtlasHeight);

                if (AtlasCollection.ContainsKey(atlasIndex))
                {
                    return AtlasCollection[atlasIndex].Tiles[x, y];
                }
                else if(MapRepository.Contains(atlasIndex))
                {
                    var exsistAtlas = MapRepository.GetAtlas(atlasIndex);
                    AtlasCollection[atlasIndex] = exsistAtlas;
                    return exsistAtlas.Tiles[x, y];
                }
                else if(AllowExpanding)
                {
                    var newAtlas = MapGenerator.Generate(atlasIndex.X, atlasIndex.Y);
                    AtlasCollection[atlasIndex] = newAtlas;
                    MapRepository.PutAtlas(atlasIndex, newAtlas);
                    return newAtlas.Tiles[x, y];

                }
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Get tile by position.
        /// </summary>
        /// <param name="pos">Tile's position</param>
        /// <returns>Tile you want.</returns>
        /// <exception cref="IndexOutOfRangeException">Tile isn't exsist and expanding is not allowed.</exception>
        public Tile this[(int X, int Y) pos] { get => this[pos.X, pos.Y]; }

        /// <summary>
        /// Config a map data container.
        /// </summary>
        /// <param name="mapGenerator">Object to generate new atlas.</param>
        /// <param name="mapRepository">Object to generate load exsisted atlas.</param>
        public MapData(IMapGenerator mapGenerator, IMapRepository mapRepository)
        {
            MapGenerator = mapGenerator;
            MapRepository = mapRepository;
        }
    }
}
