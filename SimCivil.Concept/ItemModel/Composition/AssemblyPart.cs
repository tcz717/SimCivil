using System;
using System.Collections.Generic;
using System.Linq;

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

        public IDictionary<string, IPhysicalPart> Parts { get; set; }

        public double Weight
            => Parts.Values.Sum(part => part.Weight);

        /// <summary>
        /// Gets or sets the volume. This volume has no (or not too much) relation with inner parts, because assembly has its own shape.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public double Volume { get; set; }

        /// <summary>
        /// Gets or the overall quality.
        /// </summary>
        /// <value>
        /// The quality.
        /// </value>
        public double Quality
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
