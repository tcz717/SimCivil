using SimCivil.Orleans.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Orleans.Interfaces.Component.State
{
    public class ContainerState
    {
        public IEnumerable<IEntity> Contents { get; set; }
    }
}
