using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.ApiClients
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IApiClient _apiClient;
        private readonly IUserPrincipal _loggedInUser;

        public AuthenticationService(IApiClient apiClient, IUserPrincipal _loggedInUser)
        {
            _apiClient = apiClient;
            this._loggedInUser = _loggedInUser;
        }

        public async Task<AuthenticationModel> AuthenticateUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new
                    ArgumentException("username and password should have values other than white space.");
            }

            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            using (var response = await _apiClient.Client.PostAsync("Token", data))
            {
                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.BadRequest)
                {
                    throw new Exception(response.ReasonPhrase);
                }

                // If it is Bad Request, then read the error into the model.
                var authentication = await response.Content.ReadAsAsync<AuthenticationModel>();
                return authentication;
            }
        }

        public async Task LoadLoggedInUserInfoAsync(string token)
        {
            AddAuthorizationRequestHeader(token);

            using (var response = await _apiClient.Client.GetAsync("Users/Info"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{response.ReasonPhrase}, authorization token might have been expired or currupted.");
                }

                var userInfo = await response.Content.ReadAsAsync<UserPrincipal>();

                // Fill in the model that is in the DI.
                _loggedInUser.Id = userInfo.Id;
                _loggedInUser.Token = token;
                _loggedInUser.FirstName = userInfo.FirstName;
                _loggedInUser.LastName = userInfo.LastName;
                _loggedInUser.EmailAddress = userInfo.EmailAddress;
                _loggedInUser.CreatedDate = userInfo.CreatedDate;
            }
        }

        private void AddAuthorizationRequestHeader(string token)
        {
            _apiClient.Client.DefaultRequestHeaders.Clear();
            _apiClient.Client.DefaultRequestHeaders.Add("Authorization", $"bearer {token}");

            _apiClient.Client.DefaultRequestHeaders.Accept.Clear();
            _apiClient.Client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
