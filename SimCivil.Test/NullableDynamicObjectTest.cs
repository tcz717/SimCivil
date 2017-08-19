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
    }
}
