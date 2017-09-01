using SimCivil.Auth;
using SimCivil.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Store
{
    public interface IEntityRepository
    {
        Entity LoadPlayerEntity(Player player);
    }
}
