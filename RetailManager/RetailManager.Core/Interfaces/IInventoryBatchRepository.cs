using RetailManager.Core.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailManager.Core.Interfaces
{
    public interface IInventoryBatchRepository
    {
        Task AddAsync(InventoryBatch inventoryBatch);
        Task<IEnumerable<InventoryBatch>> GetAllAsync();
    }
}