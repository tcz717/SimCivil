using System;
using System.Collections.Generic;
using System.Text;
using SimCivil.Orleans.Interfaces.Component.State;

namespace SimCivil.Concept.ItemModel
{
    public class UsableState
    {
        /// <summary>
        /// Gets or sets the weapon effects. The effect takes place on the moment a player use the item as a weapon.
        /// </summary>
        /// <value>
        /// The weapon effects.
        /// </value>
        public IList<EffectInvocation> WeaponEffects { get; set; }

        /// <summary>
        /// Gets or sets the tool effects. The effect takes place on the moment a player use the item as a tool.
        /// </summary>
        /// <value>
        /// The tool effects.
        /// </value>
        public IList<EffectInvocation> ToolEffects { get; set; }

        /// <summary>
        /// Gets or sets the universal effects. The effect takes place on the moment a player use the item without any kind of target.
        /// </summary>
        /// <value>
        /// The universal effects.
        /// </value>
        public IList<EffectInvocation> UniversalEffects { get; set; }
    }
}
