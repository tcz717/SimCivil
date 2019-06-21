using Microsoft.Extensions.Logging;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Component.State;
using System.Threading.Tasks;

namespace SimCivil.Orleans.Grains.Component
{
    public class EquipableGrain : BaseGrain<EquipableState>, IEquipable
    {
        public EquipableGrain(ILoggerFactory factory) : base(factory)
        {
            // Default
        }

        #region StateProperty

        public Task<EquipmentSlot> GetSlot()
        {
            return Task.FromResult(State.Slot);
        }

        public Task SetSlot(EquipmentSlot value)
        {
            State.Slot = value;
            return WriteStateAsync();
        }

        public Task<EffectInvocation> GetEffect()
        {
            return Task.FromResult(State.Effect);
        }

        public Task SetEffect(EffectInvocation value)
        {
            State.Effect = value;
            return WriteStateAsync();
        }

        #endregion
    }
}
