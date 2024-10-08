﻿using RetailManager.UI.Core.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

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
