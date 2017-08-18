using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Map
{
    public class MapData
    {
        public Dictionary<(int X, int Y), Atlas> AtlasCollection { get; private set; }
    }
}
