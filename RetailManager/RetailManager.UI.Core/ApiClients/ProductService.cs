using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.ApiClients
{
    public class ProductService : IProductService
    {
        private readonly IApiClient _apiClient;
        private readonly IUserPrincipal _user;

        public ProductService(IApiClient apiClient, IUserPrincipal user)
        {
            _apiClient = apiClient;
            _user = user;
        }

        public async Task<IEnumerable<ListedProductViewModel>> GetProductsAsync()
        {
            _apiClient.AddAuthorizationRequestHeaders(_user.Token);

            try
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
