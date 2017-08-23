using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static SimCivil.Config;

namespace SimCivil.Model
{
    public class Entity : ICloneable
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "Unknown";
        public (int x, int y) Position { get; set; }
        public dynamic Meta { get; set; }

        public static Entity Create()
        {
            return new Entity()
            {
                Id = Guid.NewGuid(),
                Meta = new NullableDynamicObject(),
            };
        }

        public Entity Clone()
        {
            return new Entity()
            {
                Id = Guid.NewGuid(),
                Name = Name,
                Position = Cfg.SpawnPoint,
                Meta = Meta.Clone(),
            };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
