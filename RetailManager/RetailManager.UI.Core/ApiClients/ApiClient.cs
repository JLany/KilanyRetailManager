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

        public HttpClient Client => _apiClient;

        public ApiClient()
        {
            InitializeClient();
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
