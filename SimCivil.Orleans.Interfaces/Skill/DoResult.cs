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
// SimCivil - SimCivil.Orleans.Interfaces - DoResult.cs
// Create Date: 2019/08/29
// Update Date: 2019/08/29

using System;
using System.Collections.Generic;
using System.Text;

using SimCivil.Contract.Model;

namespace SimCivil.Orleans.Interfaces.Skill
{
    public class DoResult
    {
        public enum DoResultKind
        {
            Success,
            NotLearned,
            OutOfRange,
            CoolingDown,
            Missed
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public DoResult(DoResultKind kind)
        {
            Kind = kind;
        }

        private DoResult(DoResultKind kind, IEnumerable<BodyPartIndex> bodyPartIndices)
        {
            Kind            = kind;
            BodyPartIndices = bodyPartIndices;
        }

        private DoResult(DoResultKind kind, IEnumerable<BodyPartIndex> bodyPartIndices, bool isDead)
        {
            Kind = kind;
            BodyPartIndices = bodyPartIndices;
            IsDead = isDead;
        }

        public DoResultKind               Kind            { get; set; }
        public IEnumerable<BodyPartIndex> BodyPartIndices { get; set; }
        public bool IsDead { get; set; }

        public static DoResult NotLearned()  => new DoResult(DoResultKind.NotLearned);
        public static DoResult Success()     => new DoResult(DoResultKind.Success);
        public static DoResult OutOfRange()  => new DoResult(DoResultKind.OutOfRange);
        public static DoResult CoolingDown() => new DoResult(DoResultKind.CoolingDown);

        public static DoResult HitBody(IEnumerable<BodyPartIndex> bodyPartIndices)
            => new DoResult(DoResultKind.Success, bodyPartIndices);

        public static DoResult Missed() => new DoResult(DoResultKind.Missed);

        public static DoResult Dead(IEnumerable<BodyPartIndex> bodyPartIndices)
            => new DoResult(DoResultKind.Success, bodyPartIndices, false);
    }
}