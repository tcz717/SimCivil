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
// SimCivil - SimCivil.Orleans.Grains - AccountGrain.cs
// Create Date: 2018/05/13
// Update Date: 2018/06/13

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Runtime;

using SimCivil.Contract;
using SimCivil.Orleans.Grains.State;
using SimCivil.Orleans.Interfaces;

namespace SimCivil.Orleans.Grains
{
    public class AccountGrain : Grain<AccountState>, IAccount
    {
        public ILogger<AccountGrain> Logger { get; }

        public AccountGrain(ILogger<AccountGrain> logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Determines whether this instance is exsisted.
        /// </summary>
        /// <returns></returns>
        public Task<bool> IsExisted()
        {
            return Task.FromResult(State?.Enabled ?? false);
        }

        /// <summary>
        /// Registers the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<bool> Register(string token)
        {
            if (await IsExisted())
                return false;

            State = new AccountState(token);
            await WriteStateAsync();

            Logger.Info($"Account {this.GetPrimaryKeyString()} registered");

            return true;
        }

        /// <summary>
        /// Logins the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<bool> Login(string token)
        {
            Logger.Info($"Account {this.GetPrimaryKeyString()} trying login");

            if (!await IsExisted())
            {
                Logger.Info($"Account {this.GetPrimaryKeyString()} login fail (not exist)");

                return false;
            }

            if (token != State.Token)
            {
                Logger.Info($"Account {this.GetPrimaryKeyString()} login fail (wrong Token)");

                return false;
            }

            State.Online = true;
            State.LastOnlineTime = DateTime.Now;
            await WriteStateAsync();
#pragma warning disable 4014
            GrainFactory.GetGrain<IGame>(0).OnAccountLogin(this);
#pragma warning restore 4014

            Logger.Info($"Account {this.GetPrimaryKeyString()} login successed");

            return true;
        }

        public async Task Logout()
        {
            if (IsExisted().Result)
                return;

            State.Online = false;
            await WriteStateAsync();
#pragma warning disable 4014
            GrainFactory.GetGrain<IGame>(0).OnAccountLogout(this);
#pragma warning restore 4014
        }

        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<IEnumerable<Guid>> GetRoleList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Guid> CreateRole(CreateRoleOption option)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the current role.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Guid> GetCurrentRole()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Uses the role.
        /// </summary>
        /// <param name="roleGuid">The role unique identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task UseRole(Guid roleGuid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Releases the role.
        /// </summary>
        /// <param name="roleGuid">The role unique identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task ReleaseRole(Guid roleGuid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="roleGuid">The role unique identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task DeleteRole(Guid roleGuid)
        {
            throw new NotImplementedException();
        }
    }
}