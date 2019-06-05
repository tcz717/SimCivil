using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel.Component
{
    public interface IEatable : IItemComponent<EatableState>
    {
        Task<Result<string>> GetType();

        Task<Result> SetType(string value);

        Task<Result<int>> GetSaturation();

        Task<Result> SetSaturation(int value);

        Task<Result<int>> GetEnergy();

        Task<Result> SetEnergy(int value);

        Task<Result<int>> GetHealthFactor();

        Task<Result> SetHealthFactor(int value);

        Task<Result<string>> GetEffectFunctionId();

        Task<Result> SetEffectFunctionId(string value);
    }
}
