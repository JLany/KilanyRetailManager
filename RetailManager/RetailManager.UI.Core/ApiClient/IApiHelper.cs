using RetailManager.UI.Core.Models;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.ApiClient
{
    public interface IApiHelper
    {
        Task<AuthenticationModel> AuthenticateUserAsync(string username, string password);
    }
}