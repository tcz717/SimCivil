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
// SimCivil - SimCivil.Contract - IRoleManger.cs
// Create Date: 2018/01/06
// Update Date: 2018/01/06

using System;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Contract
{
    public interface IRoleManger
    {
        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns></returns>
        Task<bool> CreateRole(CreateRoleOption option);
        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <returns></returns>
        Task<RoleSummary[]> GetRoleList();
        /// <summary>
        /// Uses the role.
        /// </summary>
        /// <param name="eid">The eid.</param>
        /// <returns></returns>
        Task<bool> UseRole(Guid eid);
        /// <summary>
        /// Releases the role.
        /// </summary>
        /// <returns></returns>
        Task<bool> ReleaseRole();
    }
}