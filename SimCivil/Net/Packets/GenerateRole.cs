using System.Collections;
using System.Linq;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// Request generate a new role
    /// </summary>
    /// <seealso>
    ///     <cref>SimCivil.Net.Packets.Packet</cref>
    /// </seealso>
    [PacketType(PacketType.GenerateRole)]
    public class GenerateRole : Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateRole"/> class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data">dictionary storing data, consist of a string and a value</param>
        /// <param name="client">client indicating where to send to or received from</param>
        public GenerateRole(PacketType type = PacketType.Empty, Hashtable data = null,
            IServerConnection client = null) : base(type, data, client)
        {
        }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name of the role.
        /// </value>
        public string RoleName
        {
            get => GetDataProperty<string>();
            set => SetDataProperty(value);
        }

        /// <summary>
        /// Gets or sets the sex.
        /// </summary>
        /// <value>
        /// The sex.
        /// </value>
        public string Sex
        {
            get => GetDataProperty<string>();
            set => SetDataProperty(value);
        }

        /// <summary>
        /// Gets or sets the race.
        /// </summary>
        /// <value>
        /// The race.
        /// </value>
        public string Race
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
                   && !string.IsNullOrWhiteSpace(RoleName)
                   && Config.AvailableSex.Contains(Sex)
                   && Config.Cfg.AvailableRaces.Contains(Race);
        }
    }
}