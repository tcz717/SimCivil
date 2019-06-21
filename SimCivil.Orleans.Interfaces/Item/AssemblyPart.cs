// Copyright (c) 2019 TPDT
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
// SimCivil - SimCivil.Orleans.Interfaces - AssemblyPart.cs
// Update Date: 2019/6/12

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimCivil.Orleans.Interfaces.Item
{
    /// <summary>
    /// Assembly is a physical part that consists of one or more sub assembly or compound.
    /// Different assemblies with the same material may have different shapes, so Assembly has a Volume property.
    /// </summary>
    /// <seealso cref="SimCivil.Orleans.Interfaces.Item.IPhysicalPart" />
    public class AssemblyPart : IPhysicalPart
    {
        private AssemblyPart() { }

        public AssemblyPart(string name = "N/A", IDictionary<string, IPhysicalPart> parts = null, double volume = 1)
        {
            Name = name;
            if (parts != null)
                Parts = parts;
            Volume = volume;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name or type of assembly. Can be used for recognizing the item function.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the subparts.
        /// </summary>
        /// <value>
        /// The subparts.
        /// </value>
        public IDictionary<string, IPhysicalPart> Parts { get; set; } = new Dictionary<string, IPhysicalPart>();

        /// <summary>
        /// Gets the total weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public double Weight
            => Parts.Values.Sum(part => part.Weight);

        /// <summary>
        /// Gets or sets the volume. This volume has no (or not too much) relation with inner parts, because assembly has its own shape.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public double Volume { get; set; }

        /// <summary>
        /// Gets or the overall quality.
        /// </summary>
        /// <value>
        /// The quality.
        /// </value>
        public double Quality
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsEatable
            => Parts.Values.Any(p => p.IsEatable);
    }
}
