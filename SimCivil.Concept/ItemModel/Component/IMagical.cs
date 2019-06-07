using SimCivil.Orleans.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel.Component
{
    public interface IMagical : IItemComponent<MagicalState>
    {
        #region StateProperty

        Task<IDictionary<string, double>> GetElementQuantities();

        Task SetElementQuantities(IDictionary<string, double> value);

        #endregion

        /// <summary>
        /// Clears elements.
        /// </summary>
        /// <returns></returns>
        Task<Result> Clear();

        /// <summary>
        /// Adds the magic elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        Task AddElements(IDictionary<string, double> elements);

        /// <summary>
        /// Updates the magic elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        Task UpdateElements(IDictionary<string, double> elements);

        /// <summary>
        /// Removes the magic elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        Task<Result> RemoveElements(IEnumerable<string> elements);
    }
}
