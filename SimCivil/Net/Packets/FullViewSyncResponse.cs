using SimCivil.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// Response to a full view sync request.
    /// </summary>
    [PacketType(PacketType.FullViewSyncResponse)]
    public class FullViewSyncResponse : ResponsePacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FullViewSyncResponse"/> class.
        /// </summary>
        /// <param name="center">The center.</param>
        /// <param name="viewDistence">The view distence.</param>
        /// <param name="tiles">The tiles.</param>
        public FullViewSyncResponse((int x, int y) center, int viewDistence, IEnumerable<Tile> tiles)
        {
            Center = center;
            ViewDistence = viewDistence;
            Tiles = tiles;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FullViewSyncResponse"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="data">The data.</param>
        /// <param name="client">The client.</param>
        public FullViewSyncResponse(PacketType type = PacketType.Empty, Hashtable data = null,
            IServerConnection client = null)
            : base(type, data, client)
        {
        }

        /// <summary>
        /// Gets or sets the center.
        /// </summary>
        /// <value>
        /// The center.
        /// </value>
        public (int x, int y) Center
        {
            get => ((int x, int y)) Data[nameof(Center)];
            set => Data[nameof(Center)] = value;
        }

        /// <summary>
        /// Gets or sets the view distence.
        /// </summary>
        /// <value>
        /// The view distence.
        /// </value>
        public int ViewDistence
        {
            get => (int) Data[nameof(ViewDistence)];
            set => Data[nameof(ViewDistence)] = value;
        }

        /// <summary>
        /// Gets or sets the tiles.
        /// </summary>
        /// <value>
        /// The tiles.
        /// </value>
        public IEnumerable<Tile> Tiles
        {
            get => (IEnumerable<Tile>) Data[nameof(Tiles)];
            set => Data[nameof(Tiles)] = value;
        }
    }
}