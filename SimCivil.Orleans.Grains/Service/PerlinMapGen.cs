using Microsoft.Extensions.Options;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Option;
using SimCivil.Orleans.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Orleans.Grains.Service
{
    public class PerlinMapGen : IMapGenerator
    {
        public PerlinMapGen(IOptions<GameOptions> gameOptions, IOptions<PerlinMapGenOptions> mapGenOptions, SimplexNoiseGenerator simplexNoiseGenerator)
        {
            GameOptions = gameOptions;
            MapGenOptions = mapGenOptions;
            SimplexNoiseGenerator = simplexNoiseGenerator;
        }

        public IOptions<GameOptions> GameOptions { get; }
        public IOptions<PerlinMapGenOptions> MapGenOptions { get; }
        public SimplexNoiseGenerator SimplexNoiseGenerator { get; }

        public Tile[,] Generate(int seed, int x, int y, int size)
        {
            SimplexNoiseGenerator.Seed = seed;
            var tiles = new Tile[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var tileX = x * size + i;
                    var tileY = y * size + j;
                    tiles[i, j] = Tile.Create(
                        (tileX, tileY),
                        height: GetHeight(tileX, tileY));
                }
            }

            return tiles;
        }

        private int GetHeight(int x, int y)
        {
            var h = SimplexNoiseGenerator.CalcPixel2D(x, y, 10);
            return (int) h;
        }
    }
}
