using RetailManager.UI.Core.Dtos;
using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.ApiClients
{
    public class SaleService : ISaleService
    {
        private readonly IApiClient _apiClient;

        public SaleService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task PostSaleAsync(SaleDto saleDto)
        {
            using (var response = await _apiClient.Client.PostAsJsonAsync("Sales/Create", saleDto))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{response.ReasonPhrase}");
                }

                // TODO: Log successful call.
            }
        }
    }
}
