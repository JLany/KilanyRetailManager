using Refit;
using RetailManager.UI.Core.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Clients
{
    public interface IAdminClient
    {
        [Get("/Admin/UserList")]
        Task<ApiResponse<List<UserModel>>> GetAllUsersAsync();

        [Get("/Admin/RoleList")]
        Task<ApiResponse<List<RoleModel>>> GetAllRolesAsync();

        [Post("/Admin/AddToRole")]
        Task<HttpResponseMessage> AddToRoleAsync([Body] UserRoleModel userRole);
        
        [Post("/Admin/RemoveFromRole")]
        Task<HttpResponseMessage> RemoveFromRoleAsync([Body] UserRoleModel userRole);
    }
}
