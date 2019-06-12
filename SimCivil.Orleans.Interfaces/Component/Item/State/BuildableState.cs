namespace SimCivil.Orleans.Interfaces.Component.State
{
    public class BuildableState
    {
        /// <summary>
        /// Gets a value indicating whether this instance is deployed as a structure in map.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deployed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeployed { get; set; }

        /// <summary>
        /// Gets or sets the type of the structure. eg. road, table, workstation, furnace etc.
        /// </summary>
        /// <value>
        /// The type of the structure.
        /// </value>
        public string StructureType { get; set; }

        /// <summary>
        /// Gets or sets the structure size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public (int length, int width) Size { get; set; }

        /// <summary>
        /// Gets or sets the effect range.
        /// </summary>
        /// <value>
        /// The effect range.
        /// </value>
        public IRange EffectRange { get; set; }

        /// <summary>
        /// Gets or sets the range effect. The effect takes place on the moment a player step into a range of the item structure.
        /// </summary>
        /// <value>
        /// The range effect.
        /// </value>
        public EffectInvocation RangeEffect { get; set; }

        /// <summary>
        /// Gets or sets the touch effect. The effect takes place on the moment a player touch the item structure.
        /// </summary>
        /// <value>
        /// The touch effect.
        /// </value>
        public EffectInvocation TouchEffect { get; set; }

        /// <summary>
        /// Gets or sets the walk on effect. The effect takes place on the moment a player steps on the item structure.
        /// </summary>
        /// <value>
        /// The walk on effects.
        /// </value>
        public EffectInvocation WalkOnEffect { get; set; }

        /// <summary>
        /// Gets or sets the operate effect. The effect takes place on the moment a player operates the item structure.
        /// </summary>
        /// <value>
        /// The operate effect.
        /// </value>
        public EffectInvocation OperateEffect { get; set; }
    }
}
