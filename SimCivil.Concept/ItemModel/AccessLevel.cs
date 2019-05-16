using System;

namespace SimCivil.Concept.ItemModel
{
    [Flags]
    internal enum AccessLevel
    {
        Observe = 0x01,
        Manipulate = 0x02,
        Destroy = 0x04,
        Owner = 0x08, // Can change owner and group

        Admin = 0x0f
    }
}