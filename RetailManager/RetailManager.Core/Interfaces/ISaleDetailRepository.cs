using RetailManager.Core.Data.Models;
using System.Threading.Tasks;

namespace RetailManager.Core.Interfaces
{
    public interface ISaleDetailRepository
    {
        Task AddAsync(SaleDetail saleDetail);
    }
}