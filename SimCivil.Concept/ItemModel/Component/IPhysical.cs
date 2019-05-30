using SimCivil.Orleans.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel
{
    interface IPhysical : IComponent<PhysicalState>
    {
        Task<Result<IEnumerable<AssemblyPart>>> GetSubAssemblies();

        Task<Result<IEnumerable<SinglePart>>> GetCompounds();

        /// <summary>
        /// Adds the compound to this item if the item is single part.
        /// </summary>
        /// <param name="compound">The compound.</param>
        /// <param name="weight">The weight.</param>
        /// <returns></returns>
        Task<Result> AddCompound(CompoundType compound, double weight);

        /// <summary>
        /// Sets the compound in this item if the item is single part.
        /// </summary>
        /// <param name="compound">The compound.</param>
        /// <param name="weight">The weight.</param>
        /// <returns></returns>
        Task<Result> SetCompound(CompoundType compound, double weight);

        /// <summary>
        /// Removes the compound from this item if the item is single part.
        /// </summary>
        /// <param name="compound">The compound.</param>
        /// <returns></returns>
        Task<Result> RemoveCompound(CompoundType compound);

        /// <summary>
        /// Inserts the physical part. This item must be an Assembly.
        /// </summary>
        /// <param name="physicalPart">The physical part.</param>
        /// <returns></returns>
        Task<Result> AddPhysicalPart(string partName, IPhysicalPart physicalPart);

        /// <summary>
        /// Removes the physical part. This item must be an Assembly.
        /// </summary>
        /// <param name="partName">Name of the part.</param>
        /// <returns></returns>
        Task<Result> RemovePhysicalPart(string partName);

        /// <summary>
        /// Determines whether this item is assembly.
        /// </summary>
        /// <returns><c>true</c> if this item is Assembly, else <c>false</c></returns>
        Task<Result<bool>> IsAssembly();

        /// <summary>
        /// Converts to assembly. Convertable iff this item is an Assembly.
        /// </summary>
        /// <returns></returns>
        Task<Result<AssemblyPart>> ToAssemblyPart();

        /// <summary>
        /// Converts to single part. Convertable iff this item is a single part.
        /// </summary>
        /// <returns></returns>
        Task<Result<SinglePart>> ToSinglePart();

        Task<Result<double>> GetWeight();

        Task<Result<double>> GetVolume();
    }
}
