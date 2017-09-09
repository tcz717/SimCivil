using log4net;
using SimCivil.Net;
using SimCivil.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static SimCivil.Config;
using System.Collections;
using SimCivil.Net.Packets;

namespace SimCivil.Map
{
    /// <summary>
    /// Map consists of Atlas.
    /// </summary>
    public class MapData
    {
        internal static readonly ILog logger = LogManager.GetLogger(typeof(MapData));
        public Dictionary<(int X, int Y), Atlas> AtlasCollection { get; private set; }
        public IMapGenerator MapGenerator { get; private set; }
        public IMapRepository MapRepository { get; }
        public IServerListener ServerListener { get; }

        /// <summary>
        /// Whether allowing expanding when tile not exsists.
        /// </summary>
        public bool AllowExpanding { get; set; } = true;

        /// <summary>
        /// Get tile by position.
        /// </summary>
        /// <param name="x">Tile's X position</param>
        /// <param name="y">Tile's Y position</param>
        /// <returns>Tile you want.</returns>
        /// <exception cref="IndexOutOfRangeException">Tile isn't exsist and expanding is not allowed.</exception>
        public Tile this[int x, int y]
        {
            get
            {
                (int X, int Y) atlasIndex = Pos2AltasIndex((x, y));
                Atlas atlas;

                if (AtlasCollection.ContainsKey(atlasIndex))
                {
                    atlas = AtlasCollection[atlasIndex];
                }
                else if (MapRepository.Contains(atlasIndex))
                {
                    atlas = MapRepository.GetAtlas(atlasIndex);
                    AtlasCollection[atlasIndex] = atlas;
                    logger.Info($"Loaded Atlas {atlasIndex} with {MapRepository}.");
                }
                else if (AllowExpanding)
                {
                    atlas = MapGenerator.Generate(atlasIndex.X, atlasIndex.Y);
                    AtlasCollection[atlasIndex] = atlas;
                    MapRepository.PutAtlas(atlasIndex, atlas);
                    logger.Info($"Generated Atlas {atlasIndex} with {MapGenerator}.");
                }
                else
                    throw new IndexOutOfRangeException();

                return atlas.Tiles[x, y];
            }
        }

        private static (int X, int Y) Pos2AltasIndex((int x, int y) pos)
        {
            return (X: pos.x % DefaultAtlasWidth, Y: pos.y % DefaultAtlasHeight);
        }

        /// <summary>
        /// Get tile by position.
        /// </summary>
        /// <param name="pos">Tile's position</param>
        /// <returns>Tile you want.</returns>
        /// <exception cref="IndexOutOfRangeException">Tile isn't exsist and expanding is not allowed.</exception>
        public Tile this[(int X, int Y) pos] { get => this[pos.X, pos.Y]; }

        /// <summary>
        /// Config a map data container.
        /// </summary>
        /// <param name="mapGenerator">Object to generate new atlas.</param>
        /// <param name="mapRepository">Object to generate load exsisted atlas.</param>
        /// <param name="serverListener">Server to sync Map</param>
        public MapData(IMapGenerator mapGenerator, IMapRepository mapRepository, IServerListener serverListener)
        {
            MapGenerator = mapGenerator;
            MapRepository = mapRepository;
            ServerListener = serverListener;
            serverListener.RegisterPacket(PacketType.FullViewSync, FullViewSyncHandle);
        }

        private void FullViewSyncHandle(Packet pkt, ref bool isVaild)
        {
            if (pkt.Client.ContextPlayer?.CurrentRole == null)
            {
                isVaild = false;
                return;
            }
            var entity = pkt.Client.ContextPlayer.CurrentRole;
            IEnumerable<Tile> view = SelectRange(entity.Position, entity.Meta.ViewDistence);
            pkt.Reply(new FullViewSyncResponse(entity.Position, entity.Meta.ViewDistence, view));
        }

        private IEnumerable<Tile> SelectRange((int x, int y) position, int viewDistence)
        {
            (int x, int y) lt = (position.x - viewDistence, position.y - viewDistence),
                lb = (position.x - viewDistence, position.y + viewDistence),
                rt = (position.x + viewDistence, position.y - viewDistence);

            for (int i = lt.y; i < lb.y; i++)
            {
                for (int j = lt.x; j < rt.x; j++)
                {
                    yield return this[i, j];
                }
            }
        }
    }
}
