using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.ApiClients
{
    public class UserService : IUserService
    {
        private readonly IApiClient _apiClient;
        private readonly IUserPrincipal _user;

        public UserService(IApiClient apiClient, IUserPrincipal user)
        {
            _apiClient = apiClient;
            _user = user;
        }

        public async Task<IEnumerable<IdentityUserModel>> GetUsersAsync()
        {
            _apiClient.AddAuthorizationRequestHeaders(_user.Token);

            try
            {
                using (var response = await _apiClient.Client.GetAsync("User/Admin/UserList"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"{response.ReasonPhrase}, no users were found.");
                    }

                    var users = await response.Content.ReadAsAsync<IEnumerable<IdentityUserModel>>();

                    return users;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _apiClient.ClearRequestHeaders();
            }
        }
    }
}
