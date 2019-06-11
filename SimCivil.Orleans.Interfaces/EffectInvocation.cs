using System.Collections.Generic;

namespace SimCivil.Orleans.Interfaces
{
    public class EffectInvocation
    {
        public string MethodId { get; set; }

        public IDictionary<string, object> Arguments { get; set; }
    }
}
