using Microsoft.Extensions.Options;
using RetailManager.UI.Core.Configuration;
using RetailManager.UI.Core.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RetailManager.UI.Core.Services
{
    public class ApiClient : IApiClient
    {
        private HttpClient _apiClient;
        private readonly ApiSettings _apiSettings;

        public HttpClient Client => _apiClient;

        public ApiClient(IOptions<ApiSettings> apiSettings)
        {
            _apiSettings = apiSettings.Value;
            InitializeClient();
        }

        public void AddAuthorizationRequestHeaders(string token)
        {
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"bearer {token}");

            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void ClearRequestHeaders()
        {
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
        }

        private void InitializeClient()
        {
            string baseAddress = _apiSettings.ApiBaseAddress;

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
