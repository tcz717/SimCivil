using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using SimCivil.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace SimCivil.Test
{
    public class ConfigTest : IDisposable
    {
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

        public void Dispose()
        {
            Directory.Delete(WorkDirectory, true);
        }

        public ConfigTest()
        {
            Directory.CreateDirectory(WorkDirectory);
        }
    }
}
