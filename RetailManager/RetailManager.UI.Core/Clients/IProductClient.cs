using Refit;
using RetailManager.UI.Core.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Clients
{
    public interface IProductClient
    {
        [Get("/Product")]
        Task<ApiResponse<List<ProductResponse>>> GetProductsAsync();
    }
}
