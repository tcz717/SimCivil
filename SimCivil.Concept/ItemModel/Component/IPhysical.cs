using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel
{
    interface IPhysical
    {
        Task<Result<IEnumerable<Assembly>>> GetSubAssemblies();

        Task<Result<IEnumerable<Compound>>> GetCompounds();

        Task<Result> AddSubAssembly(Assembly assembly, double weight);

        Task<Result> AddCompound(Compound compound, double weight);

        Task<Result<double>> GetWeight();

        Task<Result<double>> GetVolume();
    }
}
