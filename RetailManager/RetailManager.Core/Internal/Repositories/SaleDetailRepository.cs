using RetailManager.Core.Data.Models;
using RetailManager.Core.Interfaces;
using System.Threading.Tasks;

namespace RetailManager.Core.Internal.Repositories
{
    internal class SaleDetailRepository : ISaleDetailRepository
    {
        private readonly IDatabaseConnector _db;

        public SaleDetailRepository(IDatabaseConnector db)
        {
            _db = db;
        }

        public async Task AddAsync(SaleDetail saleDetail)
        {
            saleDetail.Id = await _db
                .SaveDataAsync<int>("dbo.spSaleDetail_Insert", saleDetail);
        }
    }
}
