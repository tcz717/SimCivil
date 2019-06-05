using System.Collections.Generic;
using System.Linq;

namespace SimCivil.Concept.ItemModel
{
    public class SinglePart : IPhysicalPart
    {
        public IDictionary<string, double> CompoundWeights { get; set; }

        /// <summary>
        /// Gets the weight of all compounds.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public double Weight
            => CompoundWeights.Values.Sum();

        /// <summary>
        /// Gets or sets the volume. This volume has no (or not too much) relation with Weight and Density,
        /// because CompoundPart has its own shape
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public double Volume { get; set; }

        public double Quality { get; set; }
    }
}
