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
// SimCivil - SimCivil.Orleans.Grains - GameGrain.cs
// Create Date: 2018/06/14
// Update Date: 2018/12/13

using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Orleans;
using Orleans.Runtime;

using SimCivil.Orleans.Grains.State;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Option;
using SimCivil.Orleans.Interfaces.System;

namespace SimCivil.Orleans.Grains
{
    public class GameGrain : Grain<GameState>, IGame
    {
        public ILogger<GameGrain> Logger { get; }
        public IOptions<GameOptions> GameOptions { get; }

        public bool Initialized { get; set; }

        /// <summary>
        /// This constructor should never be invoked. We expose it so that client code (subclasses of this class) do not have to add a constructor.
        /// Client code should use the GrainFactory to get a reference to a Grain.
        /// </summary>
        public GameGrain(ILogger<GameGrain> logger, IOptions<GameOptions> gameOptions)
        {
            Logger = logger;
            GameOptions = gameOptions;
        }

        public async Task InitGame(bool isDevelopment)
        {
            State.OnlineAccounts.Clear();
            if (Initialized)
            {
                return;
            }

            Initialized = true;
            Logger.Info($"Init Game:{GameOptions.Value.Name} (SW: {GameOptions.Value.SpawnPoint})");
            if (isDevelopment)
            {
                await LoadDevelopmentConfiguration();
            }
            await WriteStateAsync();
        }

        private async Task LoadDevelopmentConfiguration()
        {
            Logger.LogInformation("Development mode on");
            Logger.LogInformation(
                $"Register {await GrainFactory.GetGrain<IAccount>("admin").Register("")}");

            var human = GrainFactory.GetGrain<IEntity>(Guid.NewGuid());
            await GrainFactory.Get<IObserver>(human).SetData(new Observer { NotifyRange = 5 });
            await human.Add<IObserver>();
            await human.Add<IPosition>();
            await GrainFactory.Get<IUnit>(human).SetData(new Unit { MoveSpeed = 1 });
            await human.Add<IUnit>();
            await human.Add<IUnitController>();

            await GrainFactory.GetGrain<IPrefabManager>(0).Set("human.init", human);

            Logger.LogInformation("Add 'human.init' prefab");
        }

        public async Task OnAccountLogin(IAccount account)
        {
            State.OnlineAccounts.Add(account);
            await WriteStateAsync();
        }

        public async Task OnAccountLogout(IAccount account)
        {
            State.OnlineAccounts.Remove(account);
            await WriteStateAsync();
        }

        /// <summary>
        /// Gets the online accounts count.
        /// </summary>
        /// <returns></returns>
        public Task<int> GetOnlineAccountsCount()
        {
            return Task.FromResult(State.OnlineAccounts.Count);
        }
    }
}