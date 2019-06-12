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
// SimCivil - SimCivil.Orleans.Interfaces - EatableState.cs
// Update Date: 2019/6/12

using System;
using System.Collections.Generic;

namespace SimCivil.Orleans.Interfaces.Item
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
