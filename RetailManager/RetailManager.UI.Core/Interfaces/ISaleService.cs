using RetailManager.UI.Core.Models.Dtos;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Interfaces
{
    public interface ISaleService
    {
        Task CreateSaleAsync(SaleDto saleDto);
    }
}