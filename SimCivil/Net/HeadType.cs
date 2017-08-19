using System;

namespace SimCivil.Net
{
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