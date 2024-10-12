using Refit;
using RetailManager.UI.Core.Models.Responses;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Clients
{
    public interface IUserClient
    {
        [Get("/User/Info")]
        Task<ApiResponse<UserInfoResponse>> GetLoggedInUserInfoAsync();
    }
}
