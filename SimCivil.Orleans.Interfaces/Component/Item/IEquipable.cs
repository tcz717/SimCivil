using SimCivil.Orleans.Interfaces.Component.State;
using System.Threading.Tasks;

namespace SimCivil.Orleans.Interfaces.Component
{
    public partial interface IEquipable : IItemComponent<EquipableState>
    {
        #region StateProperty

        Task<EquipmentSlot> GetType();

        Task SetType(EquipmentSlot value);

        Task<EffectInvocation> GetEffect();

        Task SetEffect(EffectInvocation value);

        #endregion
    }
}
