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
// SimCivil - SimCivil.Test - AutoServiceTest.cs
// Create Date: 2019/06/05
// Update Date: 2019/06/06

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SimCivil.Utilities.AutoService;

using Xunit;

namespace SimCivil.Test.Utilities
{
    public class AutoServiceTest
    {
        [Fact]
        public void AutoFindServiceTest()
        {
            IServiceCollection collection = new ServiceCollection();

            collection.AutoService(typeof(AutoServiceTest).Assembly);

            Assert.NotEmpty(collection);

            ServiceProvider serviceProvider = collection.BuildServiceProvider();

            Assert.NotNull(serviceProvider.GetService<ITestServiceB>());
        }

        [Fact]
        public void AutoFindOptionsTest()
        {
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    {"test:Dump", "abcd"},
                });
            IConfigurationRoot configuration = builder.Build();

            IServiceCollection collection = new ServiceCollection();

            collection.AutoOptions(typeof(AutoServiceTest).Assembly, configuration);

            Assert.NotEmpty(collection);

            ServiceProvider serviceProvider = collection.BuildServiceProvider();

            Assert.Equal("abcd", serviceProvider.GetService<IOptions<TestOptions>>().Value.Dump);
        }

        [Fact]
        public void AutoFindOptionsWithPathTest()
        {
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    {"test:Dump", "abcd"},
                    {"DUMP:Dump", "AAAA"},
                });
            IConfigurationRoot configuration = builder.Build();

            IServiceCollection collection = new ServiceCollection();

            collection.AutoOptions(typeof(AutoServiceTest).Assembly, configuration);

            Assert.NotEmpty(collection);

            ServiceProvider serviceProvider = collection.BuildServiceProvider();

            Assert.Equal("AAAA", serviceProvider.GetService<IOptionsSnapshot<TestOptions>>().Get("NAME").Dump);
        }
    }

    [AutoOptions("test")]
    [AutoOptions("DUMP", "NAME")]
    public class TestOptions
    {
        public string Dump { get; set; }
    }
}