using System.Collections.Generic;
using System.Linq;

namespace SimCivil.Concept.ItemModel
{
    public class SinglePart : IPhysicalPart
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name or type of the part. Can be used for recognizing the item function.
        /// </value>
        public string Name { get; set; }

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

        /// <summary>
        /// Gets or sets the state of the eatable. If this part is not eatable, this should be null.
        /// </summary>
        /// <value>
        /// The state of the eatable.
        /// </value>
        public EatableState EatableState { get; set; }

        public bool IsEatable => EatableState != null;
    }
}
