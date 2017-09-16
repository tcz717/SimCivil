using System;
using System.Collections.Generic;
using System.Dynamic;

namespace SimCivil
{
    /// <summary>
    /// DynamicObject that will return null if member not exsist.
    /// </summary>
    public class NullableDynamicObject : DynamicObject, ICloneable
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public Dictionary<string, object> Data { get; set; }

        /// <inheritdoc />
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (base.TryGetMember(binder, out result))
                return true;
            Data.TryGetValue(binder.Name, out result);
            return true;
        }

        /// <inheritdoc />
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Data[binder.Name] = value;
            return true;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SimCivil.NullableDynamicObject" /> class.
        /// </summary>
        public NullableDynamicObject()
        {
            Data = new Dictionary<string, object>();
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SimCivil.NullableDynamicObject" /> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public NullableDynamicObject(Dictionary<string, object> dictionary)
        {
            Data = dictionary;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public virtual NullableDynamicObject Clone()
        {
            return new NullableDynamicObject(new Dictionary<string, object>(Data));
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}