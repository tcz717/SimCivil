using SimCivil.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// The result of QueryRoleList
    /// </summary>
    /// <seealso>
    ///     <cref>SimCivil.Net.Packets.ResponsePacket</cref>
    /// </seealso>
    [PacketType(PacketType.QueryRoleListResponse)]
    public class QueryRoleListResponse : ResponsePacket
    {
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public IEnumerable<Entity> Roles
        {
            get => Data[nameof(Roles)] as IEnumerable<Entity>;
            set => Data[nameof(Roles)] = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryRoleListResponse"/> class.
        /// </summary>
        /// <param name="roles">The roles.</param>
        public QueryRoleListResponse(IEnumerable<Entity> roles)
        {
            Roles = roles;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryRoleListResponse"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="data">The data.</param>
        /// <param name="client">The client.</param>
        public QueryRoleListResponse(PacketType type = PacketType.Empty, Hashtable data = null,
            IServerConnection client = null)
            : base(type, data, client)
        {
        }
    }
}