using System;

namespace SimCivil.Net
{
    /// <summary>
    /// Attribute to Mark a Packet class's ParketType
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PacketTypeAttribute : Attribute
    {
        private PacketType packetType;

        public PacketTypeAttribute(PacketType packetType)
        {
            this.packetType = packetType;
        }

        public PacketType PacketType { get => packetType; }
        public bool LoginRequired { get; set; } = false;
    }
}