using SimCivil.Map;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SimCivil.Test
{
    public class NullableDynamicObjectTest
    {
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
    }
}
