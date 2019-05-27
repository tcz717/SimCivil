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

        public IDictionary<Assembly, double> SubAssemblies { get; } = new Dictionary<Assembly, double>();

        public IDictionary<Compound, double> Compounds { get; } = new Dictionary<Compound, double>();
    }
}
