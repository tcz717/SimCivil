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
// SimCivil - SimCivil.Test - ConfigTest.cs
// Create Date: 2017/08/24
// Update Date: 2018/09/29

using System;
using System.IO;
using System.Text;

using Autofac.Extras.FakeItEasy;

using SimCivil.Store;

using Xunit;

namespace SimCivil.Test
{
    public class ConfigTest : IDisposable
    {
        public ConfigTest()
        {
            Directory.CreateDirectory(WorkDirectory);
        }

        public void Dispose()
        {
            Directory.Delete(WorkDirectory, true);
        }

        private const string WorkDirectory = "configtest";

        [Fact]
        public void Repository()
        {
            Random r = new Random();
            Config old;
            GameInfo info = new GameInfo()
            {
                Name = "test",
                Seed = r.Next(),
                StoreDirectory = WorkDirectory,
            };
            using (var services = new AutoFake())
            {
                var repo = services.Resolve<ConfigRepository>();
                repo.Initialize(info);
                repo.Save(info.StoreDirectory);
                Assert.Equal(Config.Cfg.Seed, info.Seed);
                Assert.Equal(Config.Cfg.Name, info.Name);
                old = Config.Cfg;
                Config.Cfg = null;
            }

            using (var services = new AutoFake())
            {
                var repo = services.Resolve<ConfigRepository>();
                repo.Load(info.StoreDirectory);

                Assert.Equal(Config.Cfg.PlantDensity, old.PlantDensity);
                Assert.Equal(Config.Cfg.PlantHeightLimit, old.PlantHeightLimit);
                Assert.Equal(Config.Cfg.Seed, old.Seed);
                Assert.Equal(Config.Cfg.SpawnPoint, old.SpawnPoint);
                Assert.Equal(Config.Cfg.Name, old.Name);
            }
        }
    }
}