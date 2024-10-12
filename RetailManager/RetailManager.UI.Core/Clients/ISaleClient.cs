using Refit;
using RetailManager.UI.Core.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Clients
{
    public interface ISaleClient
    {
        [Post("/Sale")]
        Task<ApiResponse<SaleDto>> CreateSaleAsync([Body] SaleDto sale);
    }
}
