using RetailManager.Api.Models;
using RetailManager.Api.Models.Requests;

namespace RetailManager.Api.Interfaces
{
    public interface IUserRoleService
    {
        Task AddToRoleAsync(UserRoleRequest userRole);
        IQueryable<RoleModel> GetRoles();
        IQueryable<UserRoleModel> GetUserRoles();
        IQueryable<UserRoleModel> GetUserRoles(string userId);
        Task InitRoles();
        Task RemoveFromRoleAsync(UserRoleRequest userRole);
    }
}
