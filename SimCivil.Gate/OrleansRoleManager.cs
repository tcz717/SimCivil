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
// SimCivil - SimCivil.Gate - OrleansRoleManager.cs
// Create Date: 2018/06/14
// Update Date: 2018/06/16

using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Orleans;

using SimCivil.Contract;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Rpc;
using SimCivil.Rpc.Session;

namespace SimCivil.Gate
{
    [LoginFilter]
    public class OrleansRoleManager : IRoleManager, ISessionRequred
    {
        public IGrainFactory GrainFactory { get; }

        public OrleansRoleManager(IGrainFactory grainFactory)
        {
            GrainFactory = grainFactory;
        }

        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns></returns>
        public async Task<bool> CreateRole(CreateRoleOption option)
        {
            if (option == null) throw new ArgumentNullException(nameof(option));

            option.Name = option.Name?.Trim() ?? throw new ArgumentNullException(nameof(option.Name));
            if (option.Name.Length < 3)
            {
                throw new ArgumentOutOfRangeException(nameof(option.Name));
            }

            await Session.Value.Get<IAccount>().CreateRole(option);

            return true;
        }

        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <returns></returns>
        public async Task<RoleSummary[]> GetRoleList()
        {
            var roles = await Session.Value.Get<IAccount>().GetRoleList();

            return await Task.WhenAll(
                roles.Select(
                    async r =>
                    {
                        var unit = await GrainFactory.Get<IUnit>(r).GetData();
                        var summary = new RoleSummary
                        {
                            Id = r.GetPrimaryKey(),
                            Name = await r.GetName(),
                            Race = unit.Race,
                            Gender = unit.Gender
                        };

                        return summary;
                    }));
        }

        /// <summary>
        /// Uses the role.
        /// </summary>
        /// <param name="eid">The eid.</param>
        /// <returns></returns>
        public async Task<bool> UseRole(Guid eid)
        {
            IEntity role = GrainFactory.GetGrain<IEntity>(eid);
            await Session.Value.Get<IAccount>().UseRole(role);
            Session.Value.Set(role);

            return true;
        }

        /// <summary>
        /// Releases the role.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ReleaseRole()
        {
            throw new NotImplementedException();
        }

        public AsyncLocal<IRpcSession> Session { get; set; } = new AsyncLocal<IRpcSession>();
    }
}