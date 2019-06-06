using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel
{
    interface IUsable : IItemComponent<UsableState>
    {
        #region StateProperty

        Task<Result<IList<EffectInvocation>>> GetWeaponEffects();

        Task<Result> SetWeaponEffects(IList<EffectInvocation> value);

        Task<Result<IList<EffectInvocation>>> GetToolEffects();

        Task<Result> SetToolEffects(IList<EffectInvocation> value);

        Task<Result<IList<EffectInvocation>>> GetUniversalEffects();

        Task<Result> SetUniversalEffects(IList<EffectInvocation> value);

        #endregion
    }
}
