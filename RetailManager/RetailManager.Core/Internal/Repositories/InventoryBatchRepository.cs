using RetailManager.Core.Data.Models;
using RetailManager.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailManager.Core.Internal.Repositories
{
    internal class InventoryBatchRepository : IInventoryBatchRepository
    {
        private readonly IDatabaseConnector _db;

        public InventoryBatchRepository(IDatabaseConnector db)
        {
            _db = db;
        }

        public async Task<IEnumerable<InventoryBatch>> GetAllAsync()
        {
            var inventoryBatches = await _db.LoadDataAsync<InventoryBatch>("dbo.spInventoryBatch_GetAll");

            return inventoryBatches;
        }

        public async Task AddAsync(InventoryBatch inventoryBatch)
        {
            inventoryBatch.Id =
                await _db.SaveDataAsync<int>("dbo.spInventoryBatch_Insert", inventoryBatch);
        }
    }
}
