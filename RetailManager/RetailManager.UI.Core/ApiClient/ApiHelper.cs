using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.ApiClient
{
    public class ApiHelper : IApiHelper
    {
        private readonly ILoggedInUserModel _loggedInUser;
        private HttpClient _apiClient;

        public ApiHelper(ILoggedInUserModel loggedInUser)
        {
            InitializeClient();

            _loggedInUser = loggedInUser;
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

            using (var response = await _apiClient.PostAsync("Token", data))
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
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"bearer {token}");

            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = await _apiClient.GetAsync("Users/Info"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{response.ReasonPhrase}, authorization token might have expired or currupted.");
                }

                var userInfo = await response.Content.ReadAsAsync<LoggedInUserModel>();

                // Fill in the model that is in the DI.
                _loggedInUser.Id = userInfo.Id;
                _loggedInUser.Token = token;
                _loggedInUser.FirstName = userInfo.FirstName;
                _loggedInUser.LastName = userInfo.LastName;
                _loggedInUser.EmailAddress = userInfo.EmailAddress;
                _loggedInUser.CreatedDate = userInfo.CreatedDate;
            }
        }

        private void InitializeClient()
        {
            // TODO: Get configuration from DI.
            string baseAddress = ConfigurationManager.AppSettings["ApiBaseAddress"]
                ?? throw new InvalidOperationException("Setting 'ApiBaseAddress' was not found.");

            _apiClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
