using System;
using static SimCivil.Config;

namespace SimCivil.Map
{
    public interface IMapGenerator
    {
        int Seed { get; set; }

        Atlas Generate(int x, int y, int width = DefaultAtlasWidth, int height = DefaultAtlasHeight);

        event GeneratingEventHandler OnGenerating;
    }

    public delegate void GeneratingEventHandler(IMapGenerator sender, GeneratingEventArgs args);

    public class GeneratingEventArgs : EventArgs
    {
        public (int X, int Y) Position { get; set; }
        public int Height { get; set; }
        public Tile Tile { get; set; }
    }
}