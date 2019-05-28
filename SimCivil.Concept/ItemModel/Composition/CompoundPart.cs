using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Concept.ItemModel
{
    struct CompoundPart : IPhysicalPart
    {
        Compound Compound { get; set; }

        double Weight { get; set; }

        double IPhysicalPart.Quality { get; set; }
    }
}
