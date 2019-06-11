using SimCivil.Orleans.Interfaces.Component.State;
using System.Threading.Tasks;

namespace SimCivil.Orleans.Interfaces.Component
{
    /// <summary>
    /// Item that can be held and used by a player
    /// </summary>
    /// <seealso cref="SimCivil.Orleans.Interfaces.IComponent{T}" />
    public interface IItem : IItemComponent<ItemState>
    {
        #region StateProperty

        Task<Orleans.Interfaces.IEntity> GetContainer();

        Task SetContainer(Orleans.Interfaces.IEntity value);

        #endregion

        Task<Result> Destroy();
    }
}
