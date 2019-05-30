using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCivil.Concept.ItemModel
{
    /// <summary>
    /// Assembly is a physical part that consists of one or more sub assembly or compound.
    /// Different assemblies with the same material may have different shapes, so Assembly has a Volume property.
    /// </summary>
    /// <seealso cref="SimCivil.Concept.ItemModel.IPhysicalPart" />
    public class AssemblyPart : IPhysicalPart
    {
        public AssemblyType Type { get; set; }

        public IDictionary<string, IPhysicalPart> SubAssemblies { get; set; }

        public double Weight
            => SubAssemblies.Values.Sum(part => part.Weight);

        /// <summary>
        /// Gets or sets the volume. This volume has no (or not too much) relation with inner parts, because assembly has its own shape.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public double Volume { get; set; }

        /// <summary>
        /// Gets or sets the quality.
        /// </summary>
        /// <value>
        /// The quality.
        /// </value>
        public double Quality { get; set; }
    }
}
