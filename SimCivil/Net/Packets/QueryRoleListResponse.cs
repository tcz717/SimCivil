using SimCivil.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Net.Packets
{
    [PacketType(PacketType.QueryRoleListResponse)]
    public class QueryRoleListResponse : ResponsePacket
    {
        public IEnumerable<Entity> Roles
        {
            get { return Data[nameof(Roles)] as IEnumerable<Entity>; }
            set { Data[nameof(Roles)] = value; }
        }
        public QueryRoleListResponse(IEnumerable<Entity> roles)
        {
            Roles = roles;
        }
        public QueryRoleListResponse(PacketType type = PacketType.Empty, Hashtable data = null, IServerConnection client = null)
            : base(type, data, client)
        {
        }
    }
}
