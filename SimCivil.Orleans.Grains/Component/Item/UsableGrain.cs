using Microsoft.Extensions.Logging;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Component.State;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Orleans.Grains.Component
{
    public class UsableGrain : BaseGrain<UsableState>, IUsable
    {
        public UsableGrain(ILoggerFactory factory) : base(factory)
        {
            // Default
        }

        #region StateProperty

        public Task<IList<EffectInvocation>> GetWeaponEffects()
        {
            return Task.FromResult(State.WeaponEffects);
        }

        public Task SetWeaponEffects(IList<EffectInvocation> value)
        {
            State.WeaponEffects = value;
            return WriteStateAsync();
        }

        public Task<IList<EffectInvocation>> GetToolEffects()
        {
            return Task.FromResult(State.ToolEffects);
        }

        public Task SetToolEffects(IList<EffectInvocation> value)
        {
            State.ToolEffects = value;
            return WriteStateAsync();
        }

        public Task<IList<EffectInvocation>> GetUniversalEffects()
        {
            return Task.FromResult(State.UniversalEffects);
        }

        public Task SetUniversalEffects(IList<EffectInvocation> value)
        {
            State.UniversalEffects = value;
            return WriteStateAsync();
        }

        #endregion
    }
}
