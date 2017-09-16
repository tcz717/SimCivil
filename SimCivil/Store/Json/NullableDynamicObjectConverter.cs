using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Converters;

namespace SimCivil.Store.Json
{
    /// <summary>
    /// Converts an NullableDynamicObject to and from JSON.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.Converters.ExpandoObjectConverter" />
    public class NullableDynamicObjectConverter : CustomCreationConverter<NullableDynamicObject>
    {
        /// <summary>
        /// Creates an object which will then be populated by the serializer.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// The created object.
        /// </returns>
        public override NullableDynamicObject Create(Type objectType)
        {
            return new NullableDynamicObject();
        }
    }
}