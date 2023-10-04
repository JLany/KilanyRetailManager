using RetailManager.UI.Core.Models;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.ApiClients
{
    public interface IAuthenticationService
    {
        Task<AuthenticationModel> AuthenticateUserAsync(string username, string password);
        Task LoadLoggedInUserInfoAsync(string token);
    }
}