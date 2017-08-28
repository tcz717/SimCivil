using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// Reply OK.
    /// </summary>
    [PacketType(PacketType.Error)]
    public class OkResponse : ResponsePacket
    {
        public OkResponse(string description = "accept.")
        {
            Description = description;
        }
        public OkResponse(Hashtable data, IServerConnection client) : base(data, client) { }

        public string Description
        {
            get
            {
                return (string)Data[nameof(Description)];
            }
            set
            {
                Data[nameof(Description)] = value;
            }
        }

        public override bool Verify()
        {
            return base.Verify();
        }
    }
}
