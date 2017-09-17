using log4net;
using SimCivil.Net;
using SimCivil.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static SimCivil.Config;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using JetBrains.Annotations;
using SimCivil.Model;
using SimCivil.Net.Packets;

namespace SimCivil.Map
{
    /// <summary>
    /// Map consists of Atlas.
    /// </summary>
    public class MapData
    {
        internal static readonly ILog logger = LogManager.GetLogger(typeof(MapData));

        /// <summary>
        /// Gets the atlas collection.
        /// </summary>
        /// <value>
        /// The atlas collection.
        /// </value>
        public Dictionary<(int X, int Y), Atlas> AtlasCollection { get; } = new Dictionary<(int X, int Y), Atlas>();

        /// <summary>
        /// Gets the map generator.
        /// </summary>
        /// <value>
        /// The map generator.
        /// </value>
        public IMapGenerator MapGenerator { get; }

        /// <summary>
        /// Gets the map repository.
        /// </summary>
        /// <value>
        /// The map repository.
        /// </value>
        public IMapRepository MapRepository { get; }

        /// <summary>
        /// Gets the server listener.
        /// </summary>
        /// <value>
        /// The server listener.
        /// </value>
        public IServerListener ServerListener { get; }

        /// <summary>
        /// Whether allowing expanding when tile not exists.
        /// </summary>
        public bool AllowExpanding { get; set; } = true;

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        public ObservableCollection<Entity> Entities { get; } = new ObservableCollection<Entity>();

        /// <summary>
        /// Gets or sets the entities position dictionary.
        /// </summary>
        /// <value>
        /// The entities position dictionary.
        /// </value>
        public Dictionary<(int X, int Y), Entity> EntitiesPositionDictionary { get; } =
                    new Dictionary<(int X, int Y), Entity>();

        /// <summary>
        /// Get tile by position.
        /// </summary>
        /// <param name="x">Tile's X position</param>
        /// <param name="y">Tile's Y position</param>
        /// <returns>Tile you want.</returns>
        /// <exception cref="IndexOutOfRangeException">Tile isn't existed and expanding is not allowed.</exception>
        public Tile this[int x, int y]
        {
            get
            {
                (int X, int Y) atlasIndex = Pos2AtlasIndex((x, y));
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

        private static (int X, int Y) Pos2AtlasIndex((int x, int y) pos)
        {
            return (X: pos.x % DefaultAtlasWidth, Y: pos.y % DefaultAtlasHeight);
        }

        /// <summary>
        /// Get tile by position.
        /// </summary>
        /// <param name="pos">Tile's position</param>
        /// <returns>Tile you want.</returns>
        /// <exception cref="IndexOutOfRangeException">Tile isn't existed and expanding is not allowed.</exception>
        public Tile this[(int X, int Y) pos] => this[pos.X, pos.Y];

        /// <summary>
        /// Config a map data container.
        /// </summary>
        /// <param name="mapGenerator">Object to generate new atlas.</param>
        /// <param name="mapRepository">Object to generate load existed atlas.</param>
        /// <param name="serverListener">Server to sync Map</param>
        public MapData(IMapGenerator mapGenerator, IMapRepository mapRepository, IServerListener serverListener)
        {
            MapGenerator = mapGenerator;
            MapRepository = mapRepository;
            ServerListener = serverListener;
            serverListener.RegisterPacket(PacketType.FullViewSync, FullViewSyncHandle);

            Entities.CollectionChanged += Entities_CollectionChanged;
        }

        private void Entities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Entity item in e.NewItems.Cast<Entity>())
                    {
                        EntitiesPositionDictionary[item.Position] = item;
                        item.PositionChanged += Item_PositionChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Remove:
                    foreach (Entity item in e.OldItems.Cast<Entity>())
                    {
                        EntitiesPositionDictionary.Remove(item.Position);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Replace:
                    throw new NotSupportedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Item_PositionChanged(object sender, PropertyChangedEventArgs<(int X, int Y)> e)
        {
            EntitiesPositionDictionary.Remove(e.OldValue);
            EntitiesPositionDictionary.Add(e.NewValue, sender as Entity);
        }

        private void FullViewSyncHandle(Packet pkt, ref bool isValid)
        {
            if (pkt.Client.ContextPlayer?.CurrentRole == null)
            {
                isValid = false;
                return;
            }
            var entity = pkt.Client.ContextPlayer.CurrentRole;
            IEnumerable<Tile> view = SelectRange(entity.Position, entity.Meta.ViewDistence);
            pkt.Reply(new FullViewSyncResponse(entity.Position, entity.Meta.ViewDistence, view));
        }

        private IEnumerable<Tile> SelectRange((int x, int y) position, int viewDistance)
        {
            (int x, int y) lt = (position.x - viewDistance, position.y - viewDistance),
                lb = (position.x - viewDistance, position.y + viewDistance),
                rt = (position.x + viewDistance, position.y - viewDistance);

            for (int i = lt.y; i < lb.y; i++)
            {
                for (int j = lt.x; j < rt.x; j++)
                {
                    yield return this[i, j];
                }
            }
        }

        /// <summary>
        /// Attaches the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void AttachEntity([NotNull] Entity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Entities.Add(entity);
        }

        /// <summary>
        /// Detaches the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void DetachEntity([NotNull] Entity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Entities.Remove(entity);
        }
    }
}