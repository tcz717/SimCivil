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
        public SwitchRole(Guid roleGuid)
        {
            RoleGuid = roleGuid;
        }
        public SwitchRole(PacketType type, Hashtable data, IServerConnection client) : base(type, data, client) { }

        /// <summary>
        /// Guid of role's entity.
        /// </summary>
        public Guid RoleGuid {
            get
            {
                return (Guid)Data[nameof(RoleGuid)];
            }
            set
            {
                Data[nameof(RoleGuid)] = value;
            }
        }
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
