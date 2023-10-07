using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.ApiClients
{
    public class ApiClient : IApiClient
    {
        private HttpClient _apiClient;
        private readonly IConfiguration _config;

        public HttpClient Client => _apiClient;

        public ApiClient(IConfiguration config)
        {
            _config = config;
            InitializeClient();
        }

        private void InitializeClient()
        {
            string baseAddress = _config.GetApiBaseAddress();

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
