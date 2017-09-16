using SimCivil.Map;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace SimCivil.Test
{
    public class NullableDynamicObjectTest
    {
        private readonly ITestOutputHelper _output;

        [Fact]
        public void AddTest()
        {
            dynamic o = new NullableDynamicObject();
            var value = "123";
            o.Test = value;
            Assert.Equal(o.Test, value);
        }

        [Fact]
        public void NotFindTest()
        {
            dynamic o = new NullableDynamicObject();
            var value = "123";
            o.Test = value;
            Assert.Null(o.NotExist);
        }

        [Fact]
        public void CloneTest()
        {
            dynamic o1 = new NullableDynamicObject();
            var value = "123";
            o1.Test = value;
            dynamic o2 = o1.Clone();
            o1.Test = 0;
            int? inull = o1.test;
            Assert.False(inull.HasValue);
            Assert.Equal(value, o2.Test);
        }

        [Fact]
        public void JsonTest()
        {
            dynamic o = new NullableDynamicObject();
            o.Magic = 100;
            var memoryTraceWriter = new MemoryTraceWriter();
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                TraceWriter = memoryTraceWriter
            };
            NullableDynamicObject dynamicObject = o as NullableDynamicObject;
            Assert.NotNull(dynamicObject);

            string json = JsonConvert.SerializeObject(dynamicObject, jsonSerializerSettings);
            dynamic n = JsonConvert.DeserializeObject<NullableDynamicObject>(json, jsonSerializerSettings);
            _output.WriteLine(memoryTraceWriter.ToString());
            Assert.Equal((int) n.Magic, (int) o.Magic);
        }

        public NullableDynamicObjectTest(ITestOutputHelper output)
        {
            _output = output;
        }
    }
}