using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel
{
    /// <summary>
    /// Item that can be held and used by a player
    /// </summary>
    /// <seealso cref="SimCivil.Orleans.Interfaces.IComponent{T}" />
    public interface IItem : IItemComponent<ItemState>
    {
        #region StateProperty

        Task<Result<Orleans.Interfaces.IEntity>> GetContainer();

        Task<Result> SetContainer(Orleans.Interfaces.IEntity value);

        #endregion

        Task<Result> Destroy();
    }
}
