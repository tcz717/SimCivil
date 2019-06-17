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
    public class BuildableGrain : BaseGrain<BuildableState>, IBuildable
    {
        public BuildableGrain(ILoggerFactory factory) : base(factory)
        {
            // Default
        }

        #region StateProperty

        public Task<bool> GetIsDeployed()
        {
            return Task.FromResult(State.IsDeployed);
        }

        public Task SetIsDeployed(bool value)
        {
            State.IsDeployed = value;
            return WriteStateAsync();
        }

        public Task<string> GetStructureType()
        {
            return Task.FromResult(State.StructureType);
        }

        public Task SetStructureType(string value)
        {
            State.StructureType = value;
            return WriteStateAsync();
        }

        public Task<(int length, int width)> GetSize()
        {
            return Task.FromResult(State.Size);
        }

        public Task SetSize((int length, int width) value)
        {
            State.Size = value;
            return WriteStateAsync();
        }

        public Task<IRange> GetEffectRange()
        {
            return Task.FromResult(State.EffectRange);
        }

        public Task SetEffectRange(IRange value)
        {
            State.EffectRange = value;
            return WriteStateAsync();
        }

        public Task<EffectInvocation> GetRangeEffect()
        {
            return Task.FromResult(State.RangeEffect);
        }

        public Task SetRangeEffect(EffectInvocation value)
        {
            State.RangeEffect = value;
            return WriteStateAsync();
        }

        public Task<EffectInvocation> GetTouchEffect()
        {
            return Task.FromResult(State.TouchEffect);
        }

        public Task SetTouchEffect(EffectInvocation value)
        {
            State.TouchEffect = value;
            return WriteStateAsync();
        }

        public Task<EffectInvocation> GetWalkOnEffect()
        {
            return Task.FromResult(State.WalkOnEffect);
        }

        public Task SetWalkOnEffect(EffectInvocation value)
        {
            State.WalkOnEffect = value;
            return WriteStateAsync();
        }

        public Task<EffectInvocation> GetOperateEffect()
        {
            return Task.FromResult(State.OperateEffect);
        }

        public Task SetOperateEffect(EffectInvocation value)
        {
            State.OperateEffect = value;
            return WriteStateAsync();
        }

        #endregion
    }
}
