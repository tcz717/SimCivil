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
// SimCivil - SimCivil.Orleans.Grains - AccountState.cs
// Create Date: 2018/06/14
// Update Date: 2018/06/16

using System;
using System.Collections.Generic;
using System.Text;

using SimCivil.Orleans.Interfaces;

namespace SimCivil.Orleans.Grains.State
{
    public class AccountState
    {
        public string Token { get; set; }
        public bool Online { get; set; }
        public DateTime LastOnlineTime { get; set; }
        public bool Enabled { get; set; }
        public List<IEntity> Roles { get; set; }
        public IEntity CurrentRole { get; set; }

        public AccountState(string token)
        {
            Token = token;
            Enabled = true;
            Roles = new List<IEntity>();
        }

        public AccountState() { }
    }
}