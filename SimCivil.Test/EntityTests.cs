using System;
using Newtonsoft.Json;
using SimCivil.Model;
using SimCivil.Store;
using Xunit;

namespace SimCivil.Test
{
    public class EntityTests
    {
        [Fact]
        public void MemoryEntityRepoTest()
        {
            MemoryEntityRepo repo = new MemoryEntityRepo();
            Entity entity = Entity.Create();
            Guid id = entity.Id;
            repo.SaveEntity(entity);
            entity = repo.LoadEntity(id);
            Assert.Equal(id,entity.Id);
        }
        [Fact]
        public void JsonEntityTest()
        {
            Entity entity = Entity.Create();
            entity.Meta.Magic = 100;
            string json = JsonConvert.SerializeObject(entity);
            Entity loadEntity = JsonConvert.DeserializeObject<Entity>(json);
            Assert.Equal(entity, loadEntity);
            Assert.Equal((int)entity.Meta.Magic, (int)loadEntity.Meta.Magic);
        }
    }
}