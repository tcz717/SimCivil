using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// Occured a error, connection need to close.
    /// </summary>
    [PacketType(PacketType.Error)]
    public class ErrorResponse : ResponsePacket
    {
        static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ErrorResponse(int errorCode = 0, string description = "invaild packet")
        {
            ErrorCode = errorCode;
            Description = description;
        }
        public ErrorResponse(PacketType type, Hashtable data, IServerConnection client) : base(type, data, client) { }

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

        public override bool Verify(out string errorDesc)
        {
            return base.Verify(out errorDesc)
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
        public override void Send()
        {
            logger.Info($"Send ErrorResponse for code:{ErrorCode} desc:{Description}");
            base.Send();
        }
    }
}
