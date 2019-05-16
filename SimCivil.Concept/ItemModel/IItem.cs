using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel
{
    interface IItem : IEntity
    {
        Task<IContainer> GetContainer();
        Task PutInto(IContainer container);
        Task<IUnit> GetOwner();
        Task<IUnitGroup> GetOwnGroup();
        Task SetOwner(IUnit unit);
        Task SetOwnGroup(IUnitGroup group);
        Task<AccessLevel> GetAccessLevel();
        Task Destroy();
    }
}
