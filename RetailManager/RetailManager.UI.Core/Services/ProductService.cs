using RetailManager.UI.Core.Clients;
using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using RetailManager.UI.Core.Models.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductClient _productClient;
        private readonly IApiClient _apiClient;
        private readonly IUserPrincipal _user;

        public ProductService(
            IProductClient productClient,
            IApiClient apiClient,
            IUserPrincipal user)
        {
            _productClient = productClient;
            _apiClient = apiClient;
            _user = user;
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            var response = await _productClient.GetProductsAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.ReasonPhrase}, no products were found.");
            }

            return response.Content;
        }

        public async Task<IEnumerable<ListedProductViewModel>> GetProductsAsync()
        {
            _apiClient.AddAuthorizationRequestHeaders(_user.Token);

            try
            {
                using (var response = await _apiClient.Client.GetAsync("Product"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"{response.ReasonPhrase}, no products were found.");
                    }

                    var prodcuts = await response.Content.ReadAsAsync<IEnumerable<ListedProductViewModel>>();

                    return prodcuts;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _apiClient.ClearRequestHeaders();
            }
        }
    }
}
