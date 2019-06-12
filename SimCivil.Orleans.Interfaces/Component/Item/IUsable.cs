using SimCivil.Orleans.Interfaces.Component.State;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimCivil.Orleans.Interfaces.Component
{
    public partial interface IUsable : IItemComponent<UsableState>
    {
        #region StateProperty

        Task<IList<EffectInvocation>> GetWeaponEffects();

        Task SetWeaponEffects(IList<EffectInvocation> value);

        Task<IList<EffectInvocation>> GetToolEffects();

        Task SetToolEffects(IList<EffectInvocation> value);

        Task<IList<EffectInvocation>> GetUniversalEffects();

        Task SetUniversalEffects(IList<EffectInvocation> value);

        #endregion
    }
}
