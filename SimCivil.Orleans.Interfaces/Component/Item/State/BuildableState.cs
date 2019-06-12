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
// SimCivil - SimCivil.Orleans.Interfaces - BuildableState.cs
// Update Date: 2019/6/12

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
