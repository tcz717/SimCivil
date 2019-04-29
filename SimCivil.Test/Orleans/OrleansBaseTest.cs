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
// SimCivil - SimCivil.Test - OrleansBaseTest.cs
// Create Date: 2019/04/27
// Update Date: 2019/04/29

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bogus;

using Orleans.TestingHost;

using SimCivil.Contract;
using SimCivil.Orleans.Interfaces;

using Xunit;
using Xunit.Abstractions;

namespace SimCivil.Test.Orleans
{
    public class OrleansBaseTest : IDisposable
    {
        public OrleansFixture Fixture { get; }
        public ITestOutputHelper Output { get; }
        public TestCluster Cluster { get; }
        public List<IAccount> CreatedAccount { get; } = new List<IAccount>();

        public OrleansBaseTest(OrleansFixture fixture, ITestOutputHelper output)
        {
            Cluster = fixture.Cluster;
            Fixture = fixture;
            Output = output;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Task.WhenAll(CreatedAccount.Select(a => a.Logout()));
        }

        public async Task<IAccount> GetRegisteredAccountAsync()
        {
            var faker = new Faker();
            var account = Cluster.GrainFactory.GetGrain<IAccount>(faker.Person.UserName);
            Assert.True(await account.Register(faker.Random.AlphaNumeric(8)), "Register a account");

            CreatedAccount.Add(account);

            return account;
        }

        public async Task<IEnumerable<IAccount>> GetRegisteredAccountsAsync(int num)
        {
            Task<IAccount>[] tasks = new Task<IAccount>[num];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = GetRegisteredAccountAsync();
            }

            return await Task.WhenAll(tasks);
        }

        public async Task<IEntity> GetNewRoleAsync(CreateRoleOption option = null)
        {
            IAccount account = await this.GetRegisteredAccountAsync();
            option = option ?? new Faker<CreateRoleOption>();
            IEntity entity = await account.CreateRole(option);

            await account.UseRole(entity);

            return entity;
        }

        public async Task<IEnumerable<IEntity>> GetNewRolesAsync(int num, CreateRoleOption option = null)
        {
            Task<IEntity>[] tasks = new Task<IEntity>[num];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = GetNewRoleAsync(option);
            }

            return await Task.WhenAll(tasks);
        }
    }
}