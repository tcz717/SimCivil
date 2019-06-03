using SimCivil.Orleans.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel.Component
{
    interface IMagical : IComponent
    {
        /// <summary>
        /// Clears elements.
        /// </summary>
        /// <returns></returns>
        Task<Result> Clear();

        /// <summary>
        /// Gets the magic elements.
        /// </summary>
        /// <returns></returns>
        Task<Result<IDictionary<MagicElement, double>>> GetMagicElements();

        /// <summary>
        /// Clears and sets the magic elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        Task<Result> SetMagicElements(IDictionary<MagicElement, double> elements);

        /// <summary>
        /// Adds the magic elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        Task<Result> AddMagicElements(IDictionary<MagicElement, double> elements);

        /// <summary>
        /// Updates the magic elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        Task<Result> UpdateMagicElements(IDictionary<MagicElement, double> elements);

        /// <summary>
        /// Removes the magic elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        Task<Result> RemoveMagicElements(IEnumerable<MagicElement> elements);
    }
}
