using RetailManager.UI.Core.Models;
using RetailManager.UI.Core.Models.Results;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationModel> AuthenticateUserAsync(string username, string password);
        Task LoadLoggedInUserInfoAsync(string token);
        Task<AuthenticationResult> LoginAsync(string username, string password);
        Task LogoutAsync();
        Task<AuthenticationResult> RefreshTokenAsync();
    }
}