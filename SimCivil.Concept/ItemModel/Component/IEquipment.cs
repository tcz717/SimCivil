using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel.Component
{
    public interface IEquipment : IItemComponent<EquipmentState>
    {
        Task<Result<string>> GetType();

        Task<Result> SetType(string type);

        Task<Result<string>> GetEffectFunctionId();

        Task<Result> SetEffectFunctionId(string value);
    }
}
