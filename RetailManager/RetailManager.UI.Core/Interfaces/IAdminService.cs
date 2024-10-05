using RetailManager.UI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Interfaces
{
    public interface IAdminService
    {
        Task AddUserToRoleAsync(UserRoleModel userRole);
        Task<IEnumerable<RoleModel>> GetRolesAsync();
        Task<IEnumerable<UserModel>> GetUsersAsync();
        Task RemoveUserFromRoleAsync(UserRoleModel userRole);
    }
}
