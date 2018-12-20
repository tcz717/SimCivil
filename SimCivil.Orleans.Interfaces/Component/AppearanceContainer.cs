// Copyright (c) 2017 TPDT
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
// SimCivil - SimCivil.Orleans.Interfaces - AppearanceContainer.cs
// Create Date: 2018/12/19
// Update Date: 2018/12/19

using System;
using System.Collections.Generic;
using System.Text;

using SimCivil.Contract;

namespace SimCivil.Orleans.Interfaces.Component
{
    public class AppearanceContainer
    {
        public List<AppearanceEntry> Appearances { get; set; }

        public AppearanceContainer(params AppearanceEntry[] appearanceEntries)
        {
            Appearances = new List<AppearanceEntry>(appearanceEntries);
        }

        public AppearanceContainer()
        {
            Appearances = new List<AppearanceEntry>();
        }
    }

    public class AppearanceEntry
    {
        /// <summary>Gets or sets the Appearance type.</summary>
        /// <value>The Appearance type.</value>
        public AppearanceType Type { get; set; }
        /// <summary>Gets or sets the texture identifier.</summary>
        /// <value>The texture identifier.</value>
        public uint Id { get; set; }
        /// <summary>Gets or sets the primary color.</summary>
        /// <value>The primary color.</value>
        public uint PrimaryColor { get; set; }
        /// <summary>Gets or sets the secondary color.</summary>
        /// <value>The secondary color.</value>
        public uint SecondaryColor { get; set; }
        /// <summary>Gets or sets the object quality.</summary>
        /// <value>The object quality.</value>
        public float Quality { get; set; } = float.NaN;
        /// <summary>Gets or sets the object material.</summary>
        /// <value>The object material.</value>
        public Material Material { get; set; } = Material.None;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public AppearanceEntry(
            AppearanceType type,
            uint id,
            uint primaryColor,
            uint secondaryColor,
            float quality,
            Material material)
        {
            Type = type;
            Id = id;
            PrimaryColor = primaryColor;
            SecondaryColor = secondaryColor;
            Quality = quality;
            Material = material;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public AppearanceEntry() { }

        /// <summary>Initializes a new instance of the <see cref="AppearanceEntry"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="id">The identifier.</param>
        public AppearanceEntry(AppearanceType type, uint id)
        {
            Type = type;
            Id = id;
        }
    }
}