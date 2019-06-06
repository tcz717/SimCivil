using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel
{
    interface IBuildable : IItemComponent<BuildableState>
    {
        #region StateProperty

        Task<Result<bool>> GetIsDeployed();

        Task<Result> SetIsDeployed(bool value);

        Task<Result<string>> GetStructureType();

        Task<Result> SetStructureType(string value);

        Task<Result<(int length, int width)>> GetSize();

        Task<Result> SetSize((int length, int width) value);

        Task<Result<IRange>> GetEffectRange();

        Task<Result> SetEffectRange(IRange value);

        Task<Result<EffectInvocation>> GetRangeEffect();

        Task<Result> SetRangeEffect(EffectInvocation value);

        Task<Result<EffectInvocation>> GetTouchEffect();

        Task<Result> SetTouchEffect(EffectInvocation value);

        Task<Result<EffectInvocation>> GetWalkOnEffect();

        Task<Result> SetWalkOnEffect(EffectInvocation value);

        Task<Result<EffectInvocation>> GetOperateEffect();

        Task<Result> SetOperateEffect(EffectInvocation value);

        #endregion
    }
}
