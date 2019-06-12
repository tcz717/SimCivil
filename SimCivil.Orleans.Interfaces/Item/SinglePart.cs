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
// SimCivil - SimCivil.Orleans.Interfaces - SinglePart.cs
// Update Date: 2019/6/12

using System.Collections.Generic;
using System.Linq;

namespace SimCivil.Orleans.Interfaces.Item
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
