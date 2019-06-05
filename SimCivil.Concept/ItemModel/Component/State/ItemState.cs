using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Concept.ItemModel
{
    public class ItemState
    {
        string Name { get; set; }

        Orleans.Interfaces.IEntity Container { get; set; }
    }
}
