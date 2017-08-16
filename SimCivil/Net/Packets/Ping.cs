using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Net.Packets
{
    public class Ping : Packet
    {
        public Ping(Dictionary<string, object> data, Head head = default(Head), ServerClient client = null) : base(data, head, client)
        {
        }

        public override void Handle()
        {
            throw new NotImplementedException();
        }
    }
}
