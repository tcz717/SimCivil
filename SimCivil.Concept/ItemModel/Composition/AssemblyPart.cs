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
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name or type of assembly. Can be used for recognizing the item function.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the subparts.
        /// </summary>
        /// <value>
        /// The subparts.
        /// </value>
        public IDictionary<string, IPhysicalPart> Parts { get; set; }

        /// <summary>
        /// Gets the total weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
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

        public bool IsEatable
            => Parts.Values.Any(p => p.IsEatable);
    }
}
