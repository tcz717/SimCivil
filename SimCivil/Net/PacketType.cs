namespace SimCivil.Net
{
    /// <summary>
    /// Packet's protocol meaning.
    /// </summary>
    public enum PacketType
    {
        Empty = 0,
        Ping = 1,
        PingResponse = 2,
        Error = 3,
        OK = 4,
        Handshake = 5,
        Login = 6,
        FullViewSync = 7,
        FullViewSyncResponse = 8,
        QueryRoleList = 9,
        QueryRoleListResponse = 10,
        SwitchRole = 11,
        GenerateRole =12
    }
}