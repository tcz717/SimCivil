using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel
{
    interface IWeapon : IItemComponent<WeaponState>
    {
        Task<Result<string>> GetEffectFunctionId();

        Task<Result> SetEffectFunctionId(string value);
    }
}
