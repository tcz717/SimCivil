using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Concept.ItemModel
{
    public interface IRange
    {
        bool InRange((float, float) center, (float, float) target);

        void ScaleOut(float scale);

        float MaxRadiu { get; }

        float MaxMustInRadiu { get; }

        float MinRadiu { get; }
    }
}
