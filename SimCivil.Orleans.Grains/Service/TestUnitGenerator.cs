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
// SimCivil - SimCivil.Orleans.Grains - TestUnitGenerator.cs
// Create Date: 2019/05/08
// Update Date: 2019/05/11

using System;
using System.Text;

using SimCivil.Contract;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Service;

namespace SimCivil.Orleans.Grains.Service
{
    public class TestUnitGenerator : IUnitGenerator
    {
        public UnitState GenerateInitiateUnit(CreateRoleOption option)
        {
            var unit = new UnitState
            {
                Race   = option.Race,
                Gender = option.Gender,
            };

            for (var i = 0; i < (int) BodyPartIndex.BodyPartCount; i++)
                unit.BodyParts[i] = BodyPart.Create(1, 10);
            for (var i = 0; i < (int) AbilityIndex.AbilityCount; i++)
                unit.Abilities[i] = new UnboundedProperty();
            for (var i = 0; i < (int) EffectIndex.EffectCount; i++)
                unit.Effects[i] = new UnboundedProperty();

            return unit;
        }

        public Unit GenerateByAsexual(Unit parent) => throw new NotImplementedException();

        public Unit GenerateBySexual(Unit father, Unit mother) => throw new NotImplementedException();
    }
}