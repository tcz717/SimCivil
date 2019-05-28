using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Concept.ItemModel
{
    /// <summary>
    /// Additional information for Assembly or Compound when they are created in the game
    /// </summary>
    interface IPhysicalPart
    {
        double Quality { get; set; }
    }
}
