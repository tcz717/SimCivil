namespace SimCivil.Net
{
    public enum PacketType
    {
        Empty = 0,
        Ping = 1,
        PingResponse = 2,
        Error = 3,
        OK = 4,
        Handshake = 5,
        Login = 6,
    }
}