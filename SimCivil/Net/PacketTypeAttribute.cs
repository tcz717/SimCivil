using System;

namespace SimCivil.Net
{
    /// <summary>
    /// Attribute to Mark a Packet class's ParketType
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PacketTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PacketTypeAttribute"/> class.
        /// </summary>
        /// <param name="packetType">Type of the packet.</param>
        public PacketTypeAttribute(PacketType packetType)
        {
            PacketType = packetType;
        }

        /// <summary>
        /// Gets the type of the packet.
        /// </summary>
        /// <value>
        /// The type of the packet.
        /// </value>
        public PacketType PacketType { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [login required].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [login required]; otherwise, <c>false</c>.
        /// </value>
        public bool LoginRequired { get; set; } = false;

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public PacketDirection Direction { get; set; } = PacketDirection.Both;
    }

    /// <summary>
    /// Packet flow direction
    /// </summary>
    public enum PacketDirection
    {
        Both,
        ServerToClient,
        ClientToServer,
    }
}