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
// SimCivil - SimCivil.Orleans.Interfaces - AbilityIndex.cs
// Create Date: 2019/05/13
// Update Date: 2019/05/13

using System;
using System.Text;

namespace SimCivil.Orleans.Interfaces.Component.State
{
    public enum AbilityIndex
    {
        /// <summary>重构能力</summary>
        ReconstructionAbility,

        /// <summary>解析能力</summary>
        ResolvingAbility,

        /// <summary>感知能力</summary>
        PerceptionAbility,

        /// <summary>控制能力</summary>
        ControllingAbility,

        /// <summary>精神力</summary>
        MentalAbility,

        /// <summary>视力</summary>
        Vision,

        /// <summary>创造力</summary>
        Creativity,

        /// <summary>记忆力</summary>
        Memory,

        /// <summary>理解力</summary>
        Understanding,

        /// <summary>意识</summary>
        Consciousness,

        /// <summary>消化能力</summary>
        Digestion,

        /// <summary>反应力</summary>
        Reaction,

        /// <summary>耐力</summary>
        Endurance,

        /// <summary>毒素抗性</summary>
        Antitoxic,

        /// <summary>上肢力量</summary>
        UpperPower,

        /// <summary>下肢力量</summary>
        LowerPower,

        /// <summary>上肢灵巧</summary>
        UpperDexterity,

        /// <summary>下肢灵巧</summary>
        LowerDexterity,
        AbilityCount
    }
}