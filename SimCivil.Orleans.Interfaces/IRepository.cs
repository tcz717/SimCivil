using System.Collections;
using System.Collections.Generic;

namespace SimCivil.Orleans.Interfaces
{
    public interface IRepository<TKey, TEntity> : IReadOnlyDictionary<TKey, TEntity>
    {
        void InitRepo();
    }
}