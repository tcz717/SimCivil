using System;

namespace SimCivil.Model
{
    /// <summary>
    /// A human like role entity.
    /// </summary>
    /// <seealso>
    ///     <cref>SimCivil.Model.Entity</cref>
    /// </seealso>
    public class Human : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Human"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public Human(Guid id) : base(id)
        {
        }

        /// <summary>
        /// Gets or sets the head.
        /// </summary>
        /// <value>
        /// The head.
        /// </value>
        public CreatureHead Head { get; set; }
    }

    /// <summary>
    /// Creature's Head
    /// </summary>
    public class CreatureHead
    {
        /// <summary>
        /// The heath
        /// </summary>
        public int Health = 30;

        /// <summary>
        /// The skull health
        /// </summary>
        public int SkullHealth = 30;

        /// <summary>
        /// Gets or sets the brain health.
        /// </summary>
        /// <value>
        /// The brain health.
        /// </value>
        public int BrainHealth = 30;

    }
}