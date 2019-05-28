using SimCivil.Orleans.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel
{
    interface IPhysical : IComponent
    {
        Task<Result<IEnumerable<AssemblyPart>>> GetSubAssemblies();

        Task<Result<IEnumerable<CompoundPart>>> GetCompounds();

        Task<Result> AddSubAssembly(string partName, AssemblyPart assembly);

        Task<Result> AddCompound(string partName, CompoundPart compound);

        Task<Result> RemoveSubAssembly(string partName);

        Task<Result> RemoveCompound(string partName);

        Task<Result<Assembly>> ToAssembly();

        Task<Result<double>> GetWeight();

        Task<Result<double>> GetVolume();
    }
}
