using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// Occured a error, connection need to close.
    /// </summary>
    [PacketType(PacketType.Error)]
    public class ErrorResponse : ResponsePacket
    {
        public ErrorResponse(int errorCode = 0, string description = "invaild packet")
        {
            ErrorCode = errorCode;
            Description = description;
        }
        public ErrorResponse(Hashtable data, IServerConnection client) : base(data, client) { }

        public int ErrorCode
        {
            get
            {
                return (int)Data[nameof(ErrorCode)];
            }
            set
            {
                Data[nameof(ErrorCode)] = value;
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

        public override bool Verify()
        {
            return base.Verify()
                && Data.Contains(nameof(ErrorCode))
                && Data[nameof(ErrorCode)] is string
                && Data.Contains(nameof(Description))
                && Data[nameof(ErrorCode)] is int;
        }

        public override void Handle()
        {
            base.Handle();
            client.Close();
        }
    }
}
