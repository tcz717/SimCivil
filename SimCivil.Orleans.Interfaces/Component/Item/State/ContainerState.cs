using System.Collections.Generic;

namespace SimCivil.Orleans.Interfaces.Component.State
{
    public class ContainerState
    {
        public IEnumerable<IEntity> Contents { get; set; }
    }
}
