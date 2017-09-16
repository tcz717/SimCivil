using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// Request for change current role
    /// </summary>
    [PacketType(PacketType.SwitchRole, LoginRequired = true)]
    public class SwitchRole : Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchRole"/> class.
        /// </summary>
        /// <param name="roleGuid">The role unique identifier.</param>
        public SwitchRole(Guid roleGuid)
        {
            RoleGuid = roleGuid;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchRole"/> class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data">dictionary storing data, consist of a string and a value</param>
        /// <param name="client">client indicating where to send to or received from</param>
        public SwitchRole(PacketType type, Hashtable data, IServerConnection client) : base(type, data, client) { }

        /// <summary>
        /// Guid of role's entity.
        /// </summary>
        public Guid RoleGuid {
            get => (Guid)Data[nameof(RoleGuid)];
            set => Data[nameof(RoleGuid)] = value;
        }
        /// <summary>
        /// Verify this packet's receiving correctness.
        /// </summary>
        /// <param name="errorDesc"></param>
        /// <returns></returns>
        public override bool Verify(out string errorDesc)
        {
            if (!Data.Contains(nameof(RoleGuid)))
            {
                errorDesc = $"{nameof(RoleGuid)} field not found";
                return false;
            }
            return base.Verify(out errorDesc);
        }
    }
}
