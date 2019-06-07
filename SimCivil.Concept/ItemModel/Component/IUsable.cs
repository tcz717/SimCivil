using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel
{
    interface IUsable : IItemComponent<UsableState>
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
