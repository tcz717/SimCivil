using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// Reply OK.
    /// </summary>
    [PacketType(PacketType.OK)]
    public class OkResponse : ResponsePacket
    {
        public OkResponse(bool success = true, string description = "accept.")
        {
            Success = success;
            Description = description;
        }
        public OkResponse(PacketType type, Hashtable data, IServerConnection client) : base(type, data, client) { }

        public bool Success
        {
            get
            {
                return (bool)Data[nameof(Success)];
            }
            set
            {
                Data[nameof(Success)] = value;
            }
        }
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

        public override bool Verify(out string errorDesc)
        {
            return base.Verify(out errorDesc);
        }
    }
}
