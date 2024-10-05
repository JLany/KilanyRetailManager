using RetailManager.Core.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailManager.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetAsync(int id);
    }
}
