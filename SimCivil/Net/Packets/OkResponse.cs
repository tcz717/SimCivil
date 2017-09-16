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
        /// <summary>
        /// Initializes a new instance of the <see cref="OkResponse"/> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="description">The description.</param>
        public OkResponse(bool success = true, string description = "accept.")
        {
            Success = success;
            Description = description;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="OkResponse"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="data">The data.</param>
        /// <param name="client">The client.</param>
        public OkResponse(PacketType type, Hashtable data, IServerConnection client) : base(type, data, client) { }

        /// <summary>
        /// Gets or sets a value indicating whether result is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success
        {
            get => (bool)Data[nameof(Success)];
            set => Data[nameof(Success)] = value;
        }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get => (string)Data[nameof(Description)];
            set => Data[nameof(Description)] = value;
        }
    }
}
