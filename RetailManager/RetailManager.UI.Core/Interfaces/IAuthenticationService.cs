using RetailManager.UI.Core.Models;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationModel> AuthenticateUserAsync(string username, string password);
        void EndUserSession();
        Task LoadLoggedInUserInfoAsync(string token);
    }
}