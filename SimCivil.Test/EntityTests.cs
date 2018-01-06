// Copyright (c) 2017 TPDT
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
// SimCivil - SimCivil.Test - EntityTests.cs
// Create Date: 2017/09/16
// Update Date: 2018/01/05

using System;
using System.Text;

using Newtonsoft.Json;

using SimCivil.Model;
using SimCivil.Store;
using SimCivil.Store.Json;

using Xunit;
using Xunit.Abstractions;

namespace SimCivil.Test
{
    public class EntityTests
    {
        public EntityTests(ITestOutputHelper output)
        {
            Output = output;
        }

        public ITestOutputHelper Output { get; }

        [Fact]
        public void JsonEntityTest()
        {
            Entity entity = Entity.Create();
            entity.Meta["Magic"] = 100L;
            string json = JsonConvert.SerializeObject(entity);
            Entity loadEntity = JsonConvert.DeserializeObject<Entity>(json);
            Assert.Equal(entity, loadEntity);
            Assert.Equal(entity.Meta["Magic"], loadEntity.Meta["Magic"]);

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            settings.Converters.Add(new NullableDynamicObjectConverter());
            entity = Entity.Create();
            json = JsonConvert.SerializeObject(entity, settings);
            Output.WriteLine(json);
            Assert.Equal(entity, JsonConvert.DeserializeObject<Entity>(json));
            loadEntity = (Entity) JsonConvert.DeserializeObject(json, settings);
            Assert.Equal(entity, loadEntity);
        }

        [Fact]
        public void MemoryEntityRepoTest()
        {
            MemoryEntityRepo repo = new MemoryEntityRepo();
            Entity entity = Entity.Create();
            Guid id = entity.Id;
            repo.SaveEntity(entity);
            entity = repo.LoadEntity(id);
            Assert.Equal(id, entity.Id);
        }
    }
}