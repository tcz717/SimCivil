﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Concept.ItemModel
{
    /// <summary>
    /// This file will be generated by data model
    /// </summary>
    public enum Compound
    {
        Hydrogen,
        Oxygen,
        Water,
        Wood,
        Iron,
    }

    public struct CompoundProperty
    {
        public double Density;
        public IReadOnlyDictionary<Element, double> Elements;
    }

    public class CompoundData
    {
        public IReadOnlyDictionary<Compound, CompoundProperty> Properties { get; } = new Dictionary<Compound, CompoundProperty>
        {
            {
                Compound.Hydrogen, new CompoundProperty()
                {
                    Density = 0.1,
                    Elements = new Dictionary<Element, double>()
                    {
                        { Element.H, 1 }
                    },
                }
            },
            {
                Compound.Oxygen, new CompoundProperty()
                {
                    Density = 1.2,
                    Elements = new Dictionary<Element, double>()
                    {
                        { Element.O, 1 }
                    },
                }
            },
            {
                Compound.Water, new CompoundProperty()
                {
                    Density = 1000,
                    Elements = new Dictionary<Element, double>()
                    {
                        { Element.H, 2 },
                        { Element.O, 1 }
                    },
                }
            },
            {
                Compound.Wood, new CompoundProperty()
                {
                    Density = 500,
                    Elements = new Dictionary<Element, double>()
                    {
                        { Element.H, 2 },
                        { Element.O, 1 },
                        { Element.C, 3 },
                    },
                }
            },
            {
                Compound.Iron, new CompoundProperty()
                {
                    Density = 3000,
                    Elements = new Dictionary<Element, double>()
                    {
                        { Element.Fe, 2 },
                    },
                }
            },
        };

        public double GetProperty(Compound compound)
        {
            return Properties[compound].Density;
        }

        public IReadOnlyDictionary<Element, double> GetElements(Compound compound)
        {
            return Properties[compound].Elements;
        }
    }
}
