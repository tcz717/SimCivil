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
        public Dictionary<string,object> Data { get; set; }

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

        public NullableDynamicObject()
        {
            Data = new Dictionary<string, object>();
        }
        public NullableDynamicObject(Dictionary<string, object> dictionary)
        {
            Data = dictionary;
        }

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