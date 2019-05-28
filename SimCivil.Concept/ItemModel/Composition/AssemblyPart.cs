using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Concept.ItemModel
{
    public struct AssemblyPart : IPhysicalPart
    {
        /// <summary>
        /// Gets or sets the assembly.
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        Assembly Assembly { get; set; }

        /// <summary>
        /// Gets or sets the volume. This volume has no (or not too much) relation with inner parts, because assembly has its own shape
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        double Volume { get; set; }

        /// <summary>
        /// Gets or sets the quality.
        /// </summary>
        /// <value>
        /// The quality.
        /// </value>
        double IPhysicalPart.Quality { get; set; }
    }
}
