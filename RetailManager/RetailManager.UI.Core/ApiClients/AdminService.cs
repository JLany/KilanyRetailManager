using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.ApiClients
{
    public class AdminService : IAdminService
    {
        private readonly IApiClient _apiClient;
        private readonly IUserPrincipal _user;

        public AdminService(IApiClient apiClient, IUserPrincipal user)
        {
            _apiClient = apiClient;
            _user = user;
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            _apiClient.AddAuthorizationRequestHeaders(_user.Token);

            try
            {
                using (var response = await _apiClient.Client.GetAsync("Admin/UserList"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"{response.ReasonPhrase}, no users were found.");
                    }

                    var users = await response.Content.ReadAsAsync<IEnumerable<UserModel>>();

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

        public async Task<IEnumerable<RoleModel>> GetRolesAsync()
        {
            _apiClient.AddAuthorizationRequestHeaders(_user.Token);

            try
            {
                using (var response = await _apiClient.Client.GetAsync("Admin/RoleList"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"{response.ReasonPhrase}, no roles were found.");
                    }

                    var roles = await response.Content.ReadAsAsync<IEnumerable<RoleModel>>();

                    return roles;
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

        public async Task AddUserToRoleAsync(UserRoleModel userRole)
        {
            _apiClient.AddAuthorizationRequestHeaders(_user.Token);

            try
            {
                using (var response = await _apiClient.Client.PostAsJsonAsync("Admin/AddToRole", userRole))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"{response.ReasonPhrase}, no users were found.");
                    }
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

        public async Task RemoveUserFromRoleAsync(UserRoleModel userRole)
        {
            _apiClient.AddAuthorizationRequestHeaders(_user.Token);

            try
            {
                using (var response = await _apiClient.Client.PostAsJsonAsync("Admin/RemoveFromRole", userRole))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"{response.ReasonPhrase}, no users were found.");
                    }
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
