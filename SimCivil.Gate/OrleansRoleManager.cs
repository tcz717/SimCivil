using System;
using System.Text;
using System.Threading.Tasks;

using SimCivil.Contract;

namespace SimCivil.Gate {
    public class OrleansRoleManager : IRoleManager {
        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns></returns>
        public async Task<bool> CreateRole(CreateRoleOption option)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <returns></returns>
        public async Task<RoleSummary[]> GetRoleList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Uses the role.
        /// </summary>
        /// <param name="eid">The eid.</param>
        /// <returns></returns>
        public async Task<bool> UseRole(Guid eid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Releases the role.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ReleaseRole()
        {
            throw new NotImplementedException();
        }
    }
}