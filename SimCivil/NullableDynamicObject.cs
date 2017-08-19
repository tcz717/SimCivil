using System.Collections.Generic;
using System.Dynamic;

namespace SimCivil.Map
{
    /// <summary>
    /// DynamicObject that will return null if member not exsist.
    /// </summary>
    public class NullableDynamicObject : DynamicObject
    {
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (base.TryGetMember(binder, out result))
                return true;
            if (!Data.TryGetValue(binder.Name, out result))
                result = null;
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Data[binder.Name] = value;
            return true;
        }

        public virtual NullableDynamicObject Clone()
        {
            return MemberwiseClone() as NullableDynamicObject;
        }
    }
}