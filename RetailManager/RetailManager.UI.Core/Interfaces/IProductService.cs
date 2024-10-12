using RetailManager.UI.Core.Models;
using RetailManager.UI.Core.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductResponse>> GetAllProductsAsync();
    }
}