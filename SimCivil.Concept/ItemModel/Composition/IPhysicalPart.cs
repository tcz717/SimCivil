using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Concept.ItemModel
{
    /// <summary>
    /// Additional information for Assembly or Compound when they are created in the game
    /// </summary>
    public interface IPhysicalPart
    {
        double Weight { get; }

        double Volume { get; set; }

        double Quality { get; }
    }
}
