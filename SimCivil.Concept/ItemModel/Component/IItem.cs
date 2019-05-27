using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel
{
    /// <summary>
    /// Item that can be held and used by a player
    /// </summary>
    /// <seealso cref="SimCivil.Orleans.Interfaces.IComponent{T}" />
    interface IItem : IComponent
    {
        Task<Result<IEntity>> GetContainer();

        Task<Result> Destroy();
    }
}
