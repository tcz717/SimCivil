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
// SimCivil - SimCivil - RoleManager.cs
// Create Date: 2018/01/07
// Update Date: 2018/01/31

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using JetBrains.Annotations;

using log4net;

using SimCivil.Components;
using SimCivil.Contract;
using SimCivil.Contract.Model;
using SimCivil.Model;
using SimCivil.Rpc;
using SimCivil.Rpc.Session;
using SimCivil.Store;

namespace SimCivil.Auth
{
    [LoginFilter]
    internal class RoleManager : IRoleManger, ICallWarper
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected static IMapper Mapper =
            new Mapper(
                new MapperConfiguration(
                    cfg =>
                    {
                        cfg.CreateMap<UnitComponent, RoleSummary>();
                        cfg.CreateMap<CreateRoleOption, UnitComponent>();
                    }));

        public IRpcSession Session { get; }
        public IEntityManager EntityManager { get; }
        public IEntityRepository EntityRepository { get; }
        public IPrefabRepository PrefabRepository { get; }

        public List<Entity> Roles { get; set; }

        public RoleManager(
            IRpcSession session,
            IEntityManager entityManager,
            IEntityRepository entityRepository,
            IPrefabRepository prefabRepository)
        {
            Session = session;
            EntityManager = entityManager;
            EntityRepository = entityRepository;
            PrefabRepository = prefabRepository;
        }

        public void BeforeCall(IRpcSession session)
        {
            if (Roles == null)
            {
                Roles = Session.Get<Player>().Roles.Select(id => EntityRepository.LoadEntity(id)).ToList();
            }
        }

        public void AfterCall(IRpcSession session)
        {
            if (Roles != null)
            {
                Session.Get<Player>().Roles = Roles.Select(r => r.Id).ToList();
            }
        }

        public Task<bool> CreateRole([NotNull] CreateRoleOption option)
        {
            if (option == null) throw new ArgumentNullException(nameof(option));

            option.Name = option.Name?.Trim() ?? throw new ArgumentNullException(nameof(option.Name));
            if (option.Name.Length < 3)
            {
                throw new ArgumentOutOfRangeException(nameof(option.Name));
            }

            Entity role;
            switch (option.Race)
            {
                case Race.Human:
                {
                    role = PrefabRepository.GetPrefab("human.init").Clone();
                }

                    break;
                default:

                    throw new ArgumentOutOfRangeException();
            }

            Mapper.Map(option, role.Get<UnitComponent>());
            role.Name = option.Name;
            role.Get<PositionComponent>().Pos = Config.Cfg.SpawnPoint;
            EntityRepository.SaveEntity(role);
            Roles.Add(role);

            Logger.Info($"New role {option.Name} has been added");

            return Task.FromResult(true);
        }

        public Task<RoleSummary[]> GetRoleList()
        {
            var roleSummaries = Roles.Select(
                r =>
                {
                    var summary = Mapper.Map<RoleSummary>(r.Get<UnitComponent>());
                    summary.Id = r.Id;
                    summary.Name = r.Name;

                    return summary;
                });

            return Task.FromResult(roleSummaries.ToArray());
        }

        public Task<bool> UseRole(Guid eid)
        {
            if (!EntityRepository.Contains(eid))
                return Task.FromResult(false);

            Entity role = EntityRepository.LoadEntity(eid);
            OnRoleChanging(new RoleChangeArgs {Player = Session.Get<Player>(), NewEntity = role});
            Session.Set(role);
            EntityManager.AttachEntity(role);
            OnRoleChanged(new RoleChangeArgs {Player = Session.Get<Player>(), NewEntity = role});

            return Task.FromResult(true);
        }

        public Task<bool> ReleaseRole()
        {
            Entity role = Session.Get<Entity>();
            OnRoleChanging(new RoleChangeArgs { Player = Session.Get<Player>(), OldEntity = role });
            Session.Set<Entity>(null);
            EntityManager.DettachEntity(role);
            OnRoleChanged(new RoleChangeArgs { Player = Session.Get<Player>(), OldEntity = role });

            EntityRepository.SaveEntity(role);

            return Task.FromResult(true);
        }

        /// <summary>
        /// Happen when user's role changing.
        /// </summary>
        public event EventHandler<RoleChangeArgs> RoleChanging;

        /// <summary>
        /// Happen when user's role changed.
        /// </summary>
        public event EventHandler<RoleChangeArgs> RoleChanged;

        protected virtual void OnRoleChanging(RoleChangeArgs e)
        {
            RoleChanging?.Invoke(this, e);
        }

        protected virtual void OnRoleChanged(RoleChangeArgs e)
        {
            RoleChanged?.Invoke(this, e);
        }
    }
}