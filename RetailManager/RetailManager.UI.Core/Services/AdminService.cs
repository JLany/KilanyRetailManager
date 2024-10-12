using RetailManager.UI.Core.Clients;
using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminClient _adminClient;
        private readonly IApiClient _apiClient;
        private readonly IUserPrincipal _user;

        public AdminService(
            IAdminClient adminClient,
            IApiClient apiClient,
            IUserPrincipal user)
        {
            _adminClient = adminClient;
            _apiClient = apiClient;
            _user = user;
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            var response = await _adminClient.GetAllUsersAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.ReasonPhrase}, no users were found.");
            }

            return response.Content;
        }

        public async Task<IEnumerable<RoleModel>> GetRolesAsync()
        {
            var response = await _adminClient.GetAllRolesAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.ReasonPhrase}, no roles were found.");
            }

            return response.Content;
        }

        public async Task AddUserToRoleAsync(UserRoleModel userRole)
        {
            var response = await _adminClient.AddToRoleAsync(userRole);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.ReasonPhrase}, no users were found.");
            }
        }

        public async Task RemoveUserFromRoleAsync(UserRoleModel userRole)
        {
            var response = await _adminClient.RemoveFromRoleAsync(userRole);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.ReasonPhrase}, no users were found.");
            }
        }
    }
}
