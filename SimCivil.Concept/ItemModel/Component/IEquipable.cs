using SimCivil.Orleans.Interfaces.Component.State;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel.Component
{
    public interface IEquipable : IItemComponent<EquipableState>
    {
        #region StateProperty

        Task<Result<EquipmentSlot>> GetType();

        Task<Result> SetType(EquipmentSlot value);

        Task<Result<EffectInvocation>> GetEffect();

        Task<Result> SetEffect(EffectInvocation value);

        #endregion
    }
}
