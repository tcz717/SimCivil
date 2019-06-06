using System;
using System.Collections.Generic;
using System.Text;
using SimCivil.Orleans.Interfaces.Component.State;

namespace SimCivil.Concept.ItemModel
{
    public class EquipableState
    {
        /// <summary>
        /// Gets or sets the type of equipment, eg. ring, hat, bag, shoes...
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public EquipmentSlot Type { get; set; }

        /// <summary>
        /// Gets or sets the effect function identifier. The effect takes place on the moment a player wear the equipment.
        /// This function may add a property in role's equipment effects list.
        /// </summary>
        /// <value>
        /// The effect function identifier.
        /// </value>
        public EffectInvocation Effect { get; set; }
    }
}
