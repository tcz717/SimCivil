using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Net.Packets
{
    public class ErrorResponse : ResponsePacket
    {
        public ErrorResponse(int errorCode = 0, string description = "invaild packet")
        {
            ErrorCode = errorCode;
            Description = description;
        }

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
                return (string)Data[nameof(ErrorCode)];
            }
            set
            {
                Data[nameof(ErrorCode)] = value;
            }
        }

        public override void Handle()
        {
        }
    }
}
