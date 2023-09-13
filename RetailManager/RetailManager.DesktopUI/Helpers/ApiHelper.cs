using RetailManager.DesktopUI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.Helpers
{
    public class ApiHelper : IApiHelper
    {
        private HttpClient _apiClient;

        public ApiHelper()
        {
            InitializeClient();
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
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }

                var authenticationModel = await response.Content
                    .ReadAsAsync<AuthenticationModel>();

                return authenticationModel;
            }
        }

        private void InitializeClient()
        {
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
