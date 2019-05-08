using System;
using System.Collections.Generic;
using System.Text;
using SimCivil.Contract;

namespace SimCivil.Gate
{
    class GatewayConnectionControl : IConnectionControl
    {
        /// <summary>
        /// An empty request for keeping alive
        /// </summary>
        public void Noop()
        {
            // An empty request for keeping alive
        }
    }
}
