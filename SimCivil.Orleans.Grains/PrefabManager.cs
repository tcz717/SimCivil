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
// SimCivil - SimCivil.Orleans.Grains - PrefabManager.cs
// Create Date: 2018/06/15
// Update Date: 2018/06/15

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Orleans;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.System;

namespace SimCivil.Orleans.Grains
{
    public class PrefabManager : Grain<State.PrefabState>, IPrefabManager
    {
        public ILogger<PrefabManager> Logger { get; }

        public PrefabManager(ILogger<PrefabManager> logger)
        {
            Logger = logger;
        }
        public async Task<IEntity> Clone(string name, string key)
        {
            var entity = GrainFactory.GetGrain<IEntity>(Guid.NewGuid());
            if (!State.Prefabs.ContainsKey(key))
            {
                Logger.LogWarning($"{key} not find in the registered prefabs");

                return null;
            }
            IEntity prefab = State.Prefabs[key];

            await prefab.CopyTo(entity);

            return entity;
        }

        public Task Set(string key, IEntity prefab)
        {
            State.Prefabs[key] = prefab;

            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is called at the end of the process of activating a grain.
        /// It is called before any messages have been dispatched to the grain.
        /// For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            if (State.Prefabs == null)
            {
                State.Prefabs = new Dictionary<string, IEntity>();
            }

            return base.OnActivateAsync();
        }
    }
}