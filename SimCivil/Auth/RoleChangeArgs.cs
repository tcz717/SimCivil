using System;
using System.Text;

using SimCivil.Model;

namespace SimCivil.Auth {
    /// <summary>
    /// 
    /// </summary>
    public class RoleChangeArgs
    {
        /// <summary>
        /// Gets or sets the old entity.
        /// </summary>
        /// <value>
        /// The old entity.
        /// </value>
        public Entity OldEntity { get; set; }

        /// <summary>
        /// Gets or sets the new entity.
        /// </summary>
        /// <value>
        /// The new entity.
        /// </value>
        public Entity NewEntity { get; set; }

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        public Player Player { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RoleChangeArgs"/> is allowed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if allowed; otherwise, <c>false</c>.
        /// </value>
        public bool Allowed { get; set; }
    }
}