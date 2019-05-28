using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Concept.ItemModel
{
    public enum AssemblyType
    {
        WoodAxe,
        IronAxe,
    }

    class Assembly
    {
        public AssemblyType Type { get; set; }

        public IDictionary<string, AssemblyPart> SubAssemblies { get; set; } = new Dictionary<string, AssemblyPart>();

        public IDictionary<string, CompoundPart> Compounds { get; set; } = new Dictionary<string, CompoundPart>();
    }
}
