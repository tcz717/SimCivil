﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Concept.ItemModel
{
    /// <summary>
    /// Additional information for Assembly or Compound when they are created in the game
    /// </summary>
    public interface IPhysicalPart
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name or type of the part. Can be used for recognizing the item function.
        /// </value>
        string Name { get; set; }

        double Weight { get; }

        double Volume { get; set; }

        double Quality { get; }

        bool IsEatable { get; }
    }
}