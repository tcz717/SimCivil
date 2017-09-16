using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// Occurred a error, connection need to close.
    /// </summary>
    [PacketType(PacketType.Error)]
    public class ErrorResponse : ResponsePacket
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="description">The description.</param>
        public ErrorResponse(int errorCode = 0, string description = "invalid packet")
        {
            ErrorCode = errorCode;
            Description = description;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="data">The data.</param>
        /// <param name="client">The client.</param>
        public ErrorResponse(PacketType type, Hashtable data, IServerConnection client) : base(type, data, client)
        {
        }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public int ErrorCode
        {
            get => GetDataProperty<int>();
            set => SetDataProperty(value);
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get => GetDataProperty<string>();
            set => SetDataProperty(value);
        }

        /// <summary>
        /// Verify this packet's receiving correctness.
        /// </summary>
        /// <param name="errorDesc"></param>
        /// <returns></returns>
        public override bool Verify(out string errorDesc)
        {
            return base.Verify(out errorDesc)
                   && Data.Contains(nameof(ErrorCode))
                   && Data[nameof(ErrorCode)] is string
                   && Data.Contains(nameof(Description))
                   && Data[nameof(ErrorCode)] is int;
        }

        /// <summary>
        /// The method executed after clients received and pushed in the PacketReadQueue
        /// </summary>
        public override void Handle()
        {
            base.Handle();
            Client.Close();
        }

        /// <summary>
        /// Give order to send packet immediately.
        /// Note: This is a method especially for server, please do NOT use it directly!
        /// It is recommended to enqueue packets into PacketSendQueue for sending
        /// </summary>
        public override void Send()
        {
            logger.Info($"Send ErrorResponse for code:{ErrorCode} desc:{Description}");
            base.Send();
        }
    }
}