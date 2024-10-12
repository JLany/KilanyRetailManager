using Refit;
using RetailManager.UI.Core.Models;
using RetailManager.UI.Core.Models.Requests;
using RetailManager.UI.Core.Models.Responses;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Clients
{
    public interface IAuthenticationClient
    {
        [Post("/Authentication/Login")]
        Task<ApiResponse<LoginResponse>> LoginAsync([Body] LoginRequest login);

        [Post("/Authentication/Refresh")]
        Task<ApiResponse<LoginResponse>> RefreshAsync([Body] RefreshTokenRequest request);

        [Delete("/Authentication/Logout")]
        Task<HttpResponseMessage> LogoutAsync();
    }
}
