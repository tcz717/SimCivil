﻿using SimCivil.Orleans.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Concept.ItemModel {
    class ContainerState
    {
        public IEnumerable<IEntity> Contents { get; set; }
    }
}
