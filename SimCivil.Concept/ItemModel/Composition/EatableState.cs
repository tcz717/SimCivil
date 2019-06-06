using System;
using System.Collections.Generic;

namespace SimCivil.Concept.ItemModel
{
    public class EatableState
    {
        public FoodType FoodType { get; set; }

        public int Energy { get; set; }

        public int Saturation { get; set; }

        public int HealthFactor { get; set; }

        public IEnumerable<EffectInvocation> Effects { get; set; }
    }

    [Flags]
    public enum FoodType
    {
        Staple = 0x01,
        Meat = 0x02,
        Vegetable = 0x04,
        Snack = 0x08,
        Medicine = 0x10,
    }
}
