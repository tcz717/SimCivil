using Orleans;
using Orleans.TestingHost;
using SimCivil.Orleans.Grains.Component;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimCivil.Test.Orleans
{
    [Collection(ClusterCollection.Name)]
    public class ItemTest
    {
        public static readonly Dictionary<string, double> TestWater = new Dictionary<string, double>()
        {
            { "Water" , 2.0 },
            { "Dirt" , 0.1 },
        };

        public static readonly Dictionary<string, double> TestWood = new Dictionary<string, double>()
        {
            { "Wood" , 3.0 },
        };

        public static readonly Dictionary<string, IPhysicalPart> TestCupBody = new Dictionary<string, IPhysicalPart>()
        {
            { "Wall" , new SinglePart("HollowRoll", TestWood) },
            { "Bottom" , new SinglePart("Plate", TestWood) }
        };

        public static readonly Dictionary<string, IPhysicalPart> TestCup = new Dictionary<string, IPhysicalPart>()
        {
            { "Content" , new SinglePart("Water", TestWater) },
            { "CupBody" , new AssemblyPart("CupBody", TestCupBody) }
        };

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public ItemTest(OrleansFixture fixture)
        {
            Cluster = fixture.Cluster;
        }
        public TestCluster Cluster { get; }

        [Fact]
        public async Task ItemCreateTest()
        {
            var item1 = await CreateItem();
            IEntity container1 = await CreateContainer();
            item1.Get<IItem>().Result.SetContainer(container1).Wait();
            Assert.Equal(container1, item1.Get<IItem>().Result.GetContainer().Result);
            item1.Get<IItem>().Result.SetContainer(null).Wait();
            Assert.Null(item1.Get<IItem>().Result.GetContainer().Result);
        }

        [Fact]
        public async Task ContainerTest()
        {
            var item1 = await CreateItem();
            IEntity container1 = await CreateContainer();
            Guid itemGuid = item1.GetPrimaryKey();

            var ct = await container1.Get<IContainer>();
            Assert.Equal(0, ct.GetContents().Result.Count);
            await ct.PutItem(item1);
            Assert.Equal(1, ct.GetContents().Result.Count);
            Assert.Equal(itemGuid, ct.GetContents().Result.First().GetPrimaryKey());
            AssertSuccess(ct.TakeItem(item1).Result);
            Assert.Equal(0, ct.GetContents().Result.Count);

            var items = new List<IEntity>();
            for (int i = 0; i < 100; i++)
            {
                items.Add(await CreateItem());
            }

            await ct.PutItem(item1);
            await ct.PutItems(items);
            Assert.Equal(items.Count + 1, ct.GetContents().Result.Count);
            AssertSuccess(await ct.TakeItem(item1));
            Assert.Equal(items.Count, ct.GetContents().Result.Count);
            Assert.Equal(items.Count, ct.TakeAll().Result.Count);
            Assert.Equal(0, ct.GetContents().Result.Count);
        }

        [Fact]
        public async Task PhysicalTest()
        {
            var item = await CreateItem<IPhysical>();
            var phy = await item.Get<IPhysical>();

            Assert.True(await phy.IsAssembly());
            Assert.True(await phy.IsSinglePart());

            var water = TestWater["Water"];
            var dirt = TestWater["Dirt"];

            AssertSuccess(await phy.AddCompounds(new Dictionary<string, double>(TestWater)));
            AssertFail(await phy.AddPhysicalParts(TestCup));
            AssertSuccess(await phy.AddCompounds(new Dictionary<string, double>(TestWater)));
            Assert.Equal(2 * water, (await phy.GetCompounds()).Value["Water"]);
            AssertSuccess(await phy.RemoveCompounds(new string[] { "Dirt" }));
            AssertPartialComplete(await phy.RemoveCompounds(new string[] { "Dirt" }));
            AssertSuccess(await phy.UpdateCompounds(new Dictionary<string, double>(TestWater)));
            Assert.Equal(water, (await phy.GetCompounds()).Value["Water"]);
            Assert.Equal(dirt, (await phy.GetCompounds()).Value["Dirt"]);
            AssertFail(await phy.GetPhysicalParts(), PhysicalGrain.NotAssemblyPart);
            await phy.Clear();
            Assert.Empty((await phy.GetCompounds()).Value);
            Assert.Empty((await phy.GetPhysicalParts()).Value);
        }

        private async Task<IEntity> CreateContainer()
        {
            var container1 = Cluster.GrainFactory.GetGrain<IEntity>(Guid.NewGuid());
            await container1.Add<IItem>();
            await container1.Add<IContainer>();
            await container1.Enable();
            Assert.True(await container1.IsEnabled());
            return container1;
        }

        private async Task<IEntity> CreateItem()
        {
            var item1 = Cluster.GrainFactory.GetGrain<IEntity>(Guid.NewGuid());
            await item1.Add<IItem>();
            await item1.Enable();
            Assert.True(await item1.IsEnabled());
            return item1;
        }

        private async Task<IEntity> CreateItem<TComponent>() where TComponent : IComponent
        {
            var item1 = Cluster.GrainFactory.GetGrain<IEntity>(Guid.NewGuid());
            await item1.Add<IItem>();
            await item1.Add<TComponent>();
            await item1.Enable();
            Assert.True(await item1.IsEnabled());
            return item1;
        }

        private void AssertSuccess(IResult result)
        {
            Assert.Equal(ErrorCode.Success, result.Err);
        }

        private void AssertPartialComplete(IResult result)
        {
            Assert.Equal(ErrorCode.PartiallyComplete, result.Err);
        }

        private void AssertFail(IResult result)
        {
            Assert.NotEqual(ErrorCode.Success, result.Err);
        }

        private void AssertFail(IResult result, string msg)
        {
            Assert.NotEqual(ErrorCode.Success, result.Err);
            Assert.Equal(msg, result.ErrMsg);
        }
    }
}
