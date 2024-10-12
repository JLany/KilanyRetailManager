using RetailManager.UI.Core.Clients;
using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using RetailManager.UI.Core.Models.Dtos;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleClient _saleClient;
        private readonly IApiClient _apiClient;
        private readonly IUserPrincipal _user;

        public SaleService(
            ISaleClient saleClient,
            IApiClient apiClient,
            IUserPrincipal user)
        {
            _saleClient = saleClient;
            _apiClient = apiClient;
            _user = user;
        }

        public async Task CreateSaleAsync(SaleDto sale)
        {
            var response = await _saleClient.CreateSaleAsync(sale);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.ReasonPhrase}");
            }
        }

        public async Task PostSaleAsync(SaleDto saleDto)
        {
            _apiClient.AddAuthorizationRequestHeaders(_user.Token);

            try
            {
                using (var response = await _apiClient.Client.PostAsJsonAsync("Sale", saleDto))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"{response.ReasonPhrase}");
                    }

                    // TODO: Log successful call.
                }
            }
            catch
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
