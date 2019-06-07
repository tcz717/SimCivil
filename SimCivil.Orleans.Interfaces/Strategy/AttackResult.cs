﻿// Copyright (c) 2017 TPDT
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
// SimCivil - SimCivil.Orleans.Interfaces - AttackResult.cs
// Create Date: 2019/06/04
// Update Date: 2019/06/05

using System;
using System.Collections.Immutable;
using System.Text;

using Orleans.Concurrency;

using SimCivil.Orleans.Interfaces.Component.State;

namespace SimCivil.Orleans.Interfaces.Strategy
{
    [Immutable]
    public class AttackResult
    {
        public IImmutableDictionary<BodyPartIndex, Wound> Wounds { get; set; }
        public AttackResultType                           Result { get; set; }

        public static AttackResult Miss() => new AttackResult {Result = AttackResultType.Miss};

        public static AttackResult Hit(IImmutableDictionary<BodyPartIndex, Wound> wounds)
            => new AttackResult {Result = AttackResultType.Hit, Wounds = wounds};

        public static AttackResult OutOfRange() => new AttackResult {Result = AttackResultType.OutOfRange};
    }

    public enum AttackResultType
    {
        Hit,
        Miss,
        OutOfRange,
    }
}