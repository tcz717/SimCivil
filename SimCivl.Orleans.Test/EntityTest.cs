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
// SimCivil - SimCivl.Orleans.Test - EntityTest.cs
// Create Date: 2018/02/25
// Update Date: 2018/02/25

using System;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.TestingHost;

using SimCivil.Orleans.Interfaces;

namespace SimCivl.Orleans.Test
{
    [TestClass]
    public class EntityTest
    {
        public TestCluster Cluster { get; set; }

        [TestInitialize]
        public void Init()
        {
            Cluster = new TestCluster();
            Cluster.ClusterConfiguration.AddMemoryStorageProvider();
            Cluster.Deploy();
        }

        [TestMethod]
        public void ComponentManageTest()
        {
            var entity = Cluster.GrainFactory.GetGrain<IEntity>(Guid.NewGuid());
            entity.Enable().Wait();
            Assert.IsTrue(entity.IsEnabled().Result);
            var entities = Cluster.GrainFactory.GetGrain<IEntityGroup>(0).GetEntities().Result.ToArray();
            Assert.AreEqual(1, entities.Length);
            Assert.IsTrue(entities.Contains(entity.GetPrimaryKey()));
        }
    }
}