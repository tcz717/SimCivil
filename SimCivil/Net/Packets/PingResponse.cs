namespace SimCivil.Net.Packets
{
    /// <summary>
    /// A Ping packet's response.
    /// </summary>
    [PacketType(PacketType.PingResponse)]
    public class PingResponse : ResponsePacket
    {
        /// <summary>
        /// Called when received and request client object to update timeout flag.
        /// </summary>
        public override void Handle()
        {
            // No need to handle here
        }
    }
}