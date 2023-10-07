using RetailManager.UI.Core.Dtos;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Interfaces
{
    public interface ISaleService
    {
        Task PostSaleAsync(SaleDto saleDto);
    }
}