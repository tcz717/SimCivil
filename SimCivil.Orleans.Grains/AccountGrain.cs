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
// Create Date: 2018/06/13
// Update Date: 2019/04/13

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Orleans;
using Orleans.Runtime;

using SimCivil.Contract;
using SimCivil.Contract.Model;
using SimCivil.Orleans.Grains.State;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Option;
using SimCivil.Orleans.Interfaces.Service;
using SimCivil.Orleans.Interfaces.System;

namespace SimCivil.Orleans.Grains
{
    public class AccountGrain : Grain<AccountState>, IAccount
    {
        public ILogger<AccountGrain> Logger { get; }
        public IOptions<GameOptions> GameOptions { get; }
        public IUnitGenerator UnitGenerator { get; }

        public AccountGrain(
            ILogger<AccountGrain> logger,
            IOptions<GameOptions> gameOptions,
            IUnitGenerator unitGenerator)
        {
            Logger = logger;
            GameOptions = gameOptions;
            UnitGenerator = unitGenerator;
        }

        /// <summary>
        /// Determines whether this instance is existed.
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
            if (this.GetPrimaryKeyString().Length < 3)
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

            Logger.Info($"Account {this.GetPrimaryKeyString()} login success");

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
        public Task<IEnumerable<IEntity>> GetRoleList()
        {
            return Task.FromResult((IEnumerable<IEntity>) State.Roles);
        }

        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns></returns>
        public async Task<IEntity> CreateRole(CreateRoleOption option)
        {
            IEntity role;
            switch (option.Race)
            {
                case Race.Human:
                    role = await GrainFactory.GetGrain<IPrefabManager>(0).Clone(option.Name, "human.init");

                    break;
                default:

                    throw new ArgumentOutOfRangeException(nameof(option.Race));
            }

            var unit = GrainFactory.Get<IUnit>(role);
            var pos = GrainFactory.Get<IPosition>(role);

            await unit.SetData(UnitGenerator.GenerateInitiateUnit(option));
            await unit.UpdateAbilities();
            await unit.UpdateEffects();

            await pos.SetData(new Position(GameOptions.Value.SpawnPoint));
            await role.SetName(option.Name);

            State.Roles.Add(role);
            await WriteStateAsync();

            Logger.Info($"New role {option.Race}: {option.Name} ({this.GetPrimaryKeyString()}) has been added");

            return role;
        }

        public Task<IEntity> GetCurrentRole()
        {
            throw new NotImplementedException();
        }

        public async Task UseRole(IEntity role)
        {
            if (State != null && !State.Roles.Contains(role))
            {
                throw new KeyNotFoundException($"Role: {role.GetPrimaryKey()}");
            }

            if (State != null)
            {
                Logger.LogInformation(
                    "User {0} used role {1}",
                    this.GetPrimaryKeyString(),
                    await role.GetName());

                State.CurrentRole = role;
            }

            await role.Enable();
        }

        public async Task ReleaseRole()
        {
            if (State?.CurrentRole != null)
            {
                Logger.LogInformation(
                    "User {0} released role {1}",
                    this.GetPrimaryKeyString(),
                    await State.CurrentRole.GetName());
                await State.CurrentRole.Disable();
            }
        }

        public Task DeleteRole(IEntity role)
        {
            throw new NotImplementedException();
        }
    }
}