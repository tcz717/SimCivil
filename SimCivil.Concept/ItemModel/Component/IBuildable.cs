using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel
{
    interface IBuildable : IItemComponent<BuildableState>
    {
        #region StateProperty

        Task<bool> GetIsDeployed();

        Task SetIsDeployed(bool value);

        Task<string> GetStructureType();

        Task SetStructureType(string value);

        Task<(int length, int width)> GetSize();

        Task SetSize((int length, int width) value);

        Task<IRange> GetEffectRange();

        Task SetEffectRange(IRange value);

        Task<EffectInvocation> GetRangeEffect();

        Task SetRangeEffect(EffectInvocation value);

        Task<EffectInvocation> GetTouchEffect();

        Task SetTouchEffect(EffectInvocation value);

        Task<EffectInvocation> GetWalkOnEffect();

        Task SetWalkOnEffect(EffectInvocation value);

        Task<EffectInvocation> GetOperateEffect();

        Task SetOperateEffect(EffectInvocation value);

        #endregion
    }
}
