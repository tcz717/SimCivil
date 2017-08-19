using System;

namespace SimCivil.Net
{
    /// <summary>
    /// Attribute to Mark a Packet class's ParketType
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class HeadTypeAttribute : Attribute
    {
        private PacketType packetType;

        public HeadTypeAttribute(PacketType packetType)
        {
            this.packetType = packetType;
        }
        
        public PacketType PacketType { get => packetType; }
    }
}