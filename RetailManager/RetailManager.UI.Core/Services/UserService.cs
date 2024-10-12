using RetailManager.UI.Core.Clients;
using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserClient _userClient;
        private readonly IUserPrincipal _loggedInUser;

        public UserService(
            IUserClient userClient,
            IUserPrincipal userPrincipal)
        {
            _userClient = userClient;
            _loggedInUser = userPrincipal;
        }

        public async Task LoadLoggedInUserInfoAsync()
        {
            var response = await _userClient.GetLoggedInUserInfoAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new
                    Exception($"{response.ReasonPhrase}, " +
                    $"authorization token might have been expired or currupted.");
            }

            // Fill in the model that is in the DI.
            _loggedInUser.Id = response.Content.Id;
            _loggedInUser.FirstName = response.Content.FirstName;
            _loggedInUser.LastName = response.Content.LastName;
            _loggedInUser.EmailAddress = response.Content.EmailAddress;
            _loggedInUser.CreatedDate = response.Content.CreatedDate;
        }
    }
}
