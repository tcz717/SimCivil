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
// SimCivil - SimCivil.Orleans.Interfaces - Compound.cs
// Update Date: 2019/6/12

using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Orleans.Interfaces.Item
{
    public class Compound
    {
        public string Name { get; set; }
        public double Density { get; set; }
        public double MeltPoint { get; set; }
        public double BoilPoint { get; set; }
        public double FlashPoint { get; set; }
        public IReadOnlyDictionary<string, double> Elements { get; set; }
    }

    public class JsonCompoundRepository : Dictionary<string, Compound>, IRepository<string, Compound>, IDataLoader
    {
        public static JsonCompoundRepository Instance { get; } = new JsonCompoundRepository();

        private JsonCompoundRepository() { }

        public void LoadData()
        {
            throw new NotImplementedException("Load data into properties.");
        }

        public void InitRepo()
        {
            LoadData();
        }
    }
}
