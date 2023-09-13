using RetailManager.DesktopUI.Models;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.Helpers
{
    public interface IApiHelper
    {
        Task<AuthenticationModel> AuthenticateUserAsync(string username, string password);
    }
}