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
        private readonly IUserPrincipal _user;

        public SaleService(IApiClient apiClient, IUserPrincipal user)
        {
            _apiClient = apiClient;
            _user = user;
        }

        public async Task PostSaleAsync(SaleDto saleDto)
        {
            _apiClient.AddAuthorizationRequestHeaders(_user.Token);

            try
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
