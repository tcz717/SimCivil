using System;

namespace SimCivil.Concept.ItemModel
{
    public class EatableState
    {
        public FoodType FoodType { get; set; }

        public int Energy { get; set; }

        public int Saturation { get; set; }

        public int HealthFactor { get; set; }

        public string EffectFunctionId { get; set; }
    }

    [Flags]
    public enum FoodType
    {
        Staple,
        Meat,
        Vegetable,
        Snack,
        Medicine,
    }
}
