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
// SimCivil - SimCivil.Test - AccountGrainTest.cs
// Create Date: 2019/04/26
// Update Date: 2019/04/29

using System;
using System.Text;
using System.Threading.Tasks;

using Bogus;

using SimCivil.Contract;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;

using Xunit;
using Xunit.Abstractions;

namespace SimCivil.Test.Orleans
{
    [Collection(ClusterCollection.Name)]
    public class AccountGrainTest : OrleansBaseTest
    {
        public AccountGrainTest(OrleansFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

        [Fact]
        public async Task CreateRoleTest()
        {
            CreateRoleOption option = new Faker<CreateRoleOption>();
            IAccount account = await GetRegisteredAccountAsync();
            IEntity entity = await account.CreateRole(option);
            Assert.NotNull(entity);

            var components = await entity.GetComponents();
            Assert.Contains(components, component => component is IPosition);
            Assert.Contains(components, component => component is IUnit);
            Assert.Contains(components, component => component is IUnitController);

            Assert.False(await entity.IsEnabled(), "New created role should not disabled");
            Assert.Equal(option.Name, await entity.GetName());
        }

        [Fact]
        public async Task LoginNotExistedTest()
        {
            var faker = new Faker();
            var account = Cluster.GrainFactory.GetGrain<IAccount>(faker.Person.UserName);
            Assert.False(await account.Login(faker.Random.AlphaNumeric(10)), "Login without existed account");
        }

        [Fact]
        public async Task LoginTest()
        {
            var faker = new Faker();
            var account = Cluster.GrainFactory.GetGrain<IAccount>(faker.Person.UserName);
            string token = faker.Random.AlphaNumeric(10);
            bool result = await account.Register(token);
            Assert.True(result);
            Assert.True(await account.Login(token), "Login");
        }

        [Fact]
        public async Task LoginWrongTokenTest()
        {
            var faker = new Faker();
            var account = Cluster.GrainFactory.GetGrain<IAccount>(faker.Person.UserName);
            string token = faker.Random.AlphaNumeric(10);
            bool result = await account.Register(token);
            Assert.True(result);
            Assert.False(await account.Login(faker.Random.AlphaNumeric(10)), "Login with wrong token");
        }

        [Fact]
        public async Task LogoutNotExistedTest()
        {
            var faker = new Faker();
            var account = Cluster.GrainFactory.GetGrain<IAccount>(faker.Person.UserName);
            await account.Logout();
        }

        [Fact]
        public async Task LogoutTest()
        {
            IAccount account = await GetRegisteredAccountAsync();
            await account.Logout();
        }

        [Fact]
        public async Task RegisterExistedTest()
        {
            var faker = new Faker();
            var account = Cluster.GrainFactory.GetGrain<IAccount>(faker.Person.UserName);
            Assert.True(await account.Register(faker.Random.AlphaNumeric(10)));
            Assert.False(await account.Register(faker.Random.AlphaNumeric(10)));
        }

        [Fact]
        public async Task RegisterShortUsernameTest()
        {
            var faker = new Faker();
            var account = Cluster.GrainFactory.GetGrain<IAccount>(faker.Random.AlphaNumeric(2));
            bool result = await account.Register(faker.Random.AlphaNumeric(10));
            Assert.False(result);
        }

        [Fact]
        public async Task RegisterTest()
        {
            var faker = new Faker();
            var account = Cluster.GrainFactory.GetGrain<IAccount>(faker.Person.UserName);
            bool result = await account.Register(faker.Random.AlphaNumeric(10));
            Assert.True(result);
        }

        [Fact]
        public async Task ReleaseRoleTest()
        {
            CreateRoleOption option = new Faker<CreateRoleOption>();
            IAccount account = await GetRegisteredAccountAsync();
            IEntity entity = await account.CreateRole(option);
            Assert.NotNull(entity);

            Assert.False(await entity.IsEnabled(), "New created role should not disabled");
            await account.UseRole(entity);
            Assert.True(await entity.IsEnabled(), "Role now is used");

            await account.ReleaseRole();
            Assert.False(await entity.IsEnabled(), "Role now is released");
        }

        [Fact]
        public async Task ReleaseUnusedRoleTest()
        {
            IAccount account = await GetRegisteredAccountAsync();
            await account.ReleaseRole();
        }

        [Fact]
        public async Task UseRoleTask()
        {
            CreateRoleOption option = new Faker<CreateRoleOption>();
            IAccount account = await GetRegisteredAccountAsync();
            IEntity entity = await account.CreateRole(option);
            Assert.NotNull(entity);

            Assert.False(await entity.IsEnabled(), "New created role should not disabled");
            await account.UseRole(entity);
            Assert.True(await entity.IsEnabled(), "Role now is used");
        }
    }
}