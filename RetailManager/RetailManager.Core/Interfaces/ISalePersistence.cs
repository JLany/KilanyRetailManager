using RetailManager.Core.Data.Dtos;
using RetailManager.Core.Data.Models;
using System.Threading.Tasks;

namespace RetailManager.Core.Interfaces
{
    public interface ISalePersistence
    {
        Task<Sale> Create(SaleDto saleDto, string cashierId);
    }
}