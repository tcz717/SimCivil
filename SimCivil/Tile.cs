using Newtonsoft.Json;
using static SimCivil.Config;

namespace SimCivil.Map
{
    public class Tile
    {
        public int Height { get; set; }
        public Slot[] Slots { get; set; }
        public Side[] Sides { get; set; }
        public string Surface { get; set; }
        public dynamic Meta { get; set; }
        
        public (int X, int Y) Position { get; set; }

        public static Tile Create((int X, int Y) pos, string surface = "", int height = SeaLevel)
        {
            var e = new Tile()
            {
                Height = height,
                Meta = new NullableDynamicObject(),
                Position = pos,
                Surface = surface,
            };
            e.Slots = new Slot[5];
            e.Sides = new Side[4];
            return e;
        }
    }
}