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
// Update Date: 2018/06/16

using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Runtime;

using SimCivil.Orleans.Interfaces;

namespace SimCivil.Orleans.Grains
{
    public class GameGrain : Grain<GameState>, IGame
    {
        public ILogger<GameGrain> Logger { get; }

        /// <summary>
        /// This constructor should never be invoked. We expose it so that client code (subclasses of this class) do not have to add a constructor.
        /// Client code should use the GrainFactory to get a reference to a Grain.
        /// </summary>
        public GameGrain(ILogger<GameGrain> logger)
        {
            Logger = logger;
        }

        public async Task InitGame(Config config)
        {
            if (State.Config != null)
            {
                Logger.Warn(0, "An existed game config has been overwritten");
            }

            State = new GameState {Config = config};
            Logger.Info($"Init Game:{config.Name} (SW: {config.SpawnPoint})");
            await WriteStateAsync();
        }

        public Task<Config> GetConfig()
        {
            if (State.Config == null)
            {
                throw new InvalidOperationException("Game is not initilized");
            }

            return Task.FromResult(State.Config);
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