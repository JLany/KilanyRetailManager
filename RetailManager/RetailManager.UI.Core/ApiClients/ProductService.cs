using Newtonsoft.Json.Linq;
using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.ApiClients
{
    public class ProductService : IProductService
    {
        private readonly IApiClient _apiClient;

        public ProductService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IEnumerable<ListedProductViewModel>> GetProductsAsync()
        {

            using (var response = await _apiClient.Client.GetAsync("Products/Get"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{response.ReasonPhrase}, no products were found.");
                }

                var prodcuts = await response.Content.ReadAsAsync<IEnumerable<ListedProductViewModel>>();
                return prodcuts;
            }
        }
    }
}
