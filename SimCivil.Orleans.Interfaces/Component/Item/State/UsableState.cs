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
// SimCivil - SimCivil.Orleans.Interfaces - UsableState.cs
// Update Date: 2019/6/12

using System.Collections.Generic;

namespace SimCivil.Orleans.Interfaces.Component.State
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
