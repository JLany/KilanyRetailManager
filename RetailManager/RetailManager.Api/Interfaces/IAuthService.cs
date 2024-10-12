using RetailManager.Api.Models.Requests;
using RetailManager.Api.Models.Responses;

namespace RetailManager.Api.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest login);
        Task LogoutAsync(string? username);
        Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
