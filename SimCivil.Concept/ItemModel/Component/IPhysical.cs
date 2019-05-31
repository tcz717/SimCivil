using SimCivil.Orleans.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel
{
    interface IPhysical : IComponent<PhysicalState>
    {
        /// <summary>
        /// Clears compounds or sub physical parts.
        /// </summary>
        /// <returns></returns>
        Task<Result> Clear();

        /// <summary>
        /// Gets the compounds of this item if the item is single part.
        /// </summary>
        /// <returns></returns>
        Task<Result<IDictionary<CompoundType, double>>> GetCompounds();

        /// <summary>
        /// Clears and sets the compounds of this item if the item is single part.
        /// </summary>
        /// <param name="compounds">The compounds.</param>
        /// <returns></returns>
        Task<Result> SetCompounds(IDictionary<CompoundType, double> compounds);

        /// <summary>
        /// Adds the compounds to this item if the item is single part.
        /// If a compound exists, then adds the weight.
        /// </summary>
        /// <param name="compounds">The compound types and weights.</param>
        /// <returns></returns>
        Task<Result> AddCompounds(IDictionary<CompoundType, double> compounds);

        /// <summary>
        /// Update the compounds weight in this item if the item is single part.
        /// </summary>
        /// <param name="compounds">The compound types and weights.</param>
        /// <returns></returns>
        Task<Result> UpdateCompounds(IDictionary<CompoundType, double> compounds);

        /// <summary>
        /// Removes the compound from this item if the item is single part.
        /// The compounds may not in the item.
        /// </summary>
        /// <param name="compounds">The compounds to be removed.</param>
        /// <returns></returns>
        Task<Result> RemoveCompound(IEnumerable<CompoundType> compounds);

        /// <summary>
        /// Gets the sub physical parts of this item if the item is assembly.
        /// </summary>
        /// <returns></returns>
        Task<Result<IDictionary<string, IPhysicalPart>>> GetPhysicalParts();

        /// <summary>
        /// Clears and sets the sub physical parts of this item if the item is assembly.
        /// </summary>
        /// <param name="subparts">The sub parts.</param>
        /// <returns></returns>
        Task<Result> SetPhysicalParts(IDictionary<string, IPhysicalPart> subparts);

        /// <summary>
        /// Inserts the physical part. This item must be an Assembly.
        /// </summary>
        /// <param name="subparts">The physical part names and parts.</param>
        /// <returns></returns>
        Task<Result> AddPhysicalPart(IDictionary<string, IPhysicalPart> subparts);

        /// <summary>
        /// Update the sub physical parts weight in this item if the item is assembly.
        /// </summary>
        /// <param name="subparts">The physical part names and parts.</param>
        /// <returns></returns>
        Task<Result> UpdatePhysicalPart(IDictionary<string, IPhysicalPart> subparts);

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
