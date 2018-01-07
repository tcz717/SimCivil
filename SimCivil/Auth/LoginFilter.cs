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
// SimCivil - SimCivil - LoginFilter.cs
// Create Date: 2018/01/07
// Update Date: 2018/01/07

using System;
using System.Text;

using SimCivil.Rpc;
using SimCivil.Rpc.Filter;
using SimCivil.Rpc.Session;

namespace SimCivil.Auth
{
    /// <summary>
    /// Allowed only logged in
    /// </summary>
    /// <seealso>
    ///     <cref>SimCivil.Rpc.Filter.SessionFilterAttribute</cref>
    /// </seealso>
    public class LoginFilterAttribute : SessionFilterAttribute
    {
        /// <summary>
        /// Checks the permission.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public override CheckResult CheckPermission(IRpcSession session)
        {
            return CheckResult.If(session.IsSet<Player>(), "Not logged in");
        }
    }
}