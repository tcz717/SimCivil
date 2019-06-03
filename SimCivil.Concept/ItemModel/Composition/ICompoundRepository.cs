using System.Collections.Generic;

namespace SimCivil.Concept.ItemModel
{
    public interface ICompoundRepository
    {
        double GetProperty(CompoundType compound);

        IReadOnlyDictionary<Element, double> GetElements(CompoundType compound);
    }
}