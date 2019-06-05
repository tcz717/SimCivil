using System.Collections;
using System.Collections.Generic;

namespace SimCivil.Concept.ItemModel
{
    public interface IRepository<TKey, TEntity> : IReadOnlyDictionary<TKey, TEntity>
    {
        void InitRepo();
    }
}