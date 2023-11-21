using RetailManager.Core.Data.Models;
using RetailManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.Core.Internal.Repositories
{
    internal class SaleRepository : ISaleRepository
    {
        private readonly IDatabaseConnector _db;

        public SaleRepository(IDatabaseConnector db)
        {
            _db = db;
        }

        public async Task AddAsync(Sale sale)
        {
            sale.Id = await _db
                .SaveDataAsync<int>("dbo.spSale_Insert", sale);
        }

        public async Task UpdateAsync(Sale sale)
        {
            Sale updateCandidate = await GetAsync(sale.Id);

            if (updateCandidate is null)
            { 
                await AddAsync(sale);
                return;
            }

            updateCandidate.SubTotal = sale.SubTotal;
            updateCandidate.Tax = sale.Tax;
            updateCandidate.Total = sale.Total;

            await _db.SaveDataAsync("dbo.spSale_Update", updateCandidate);
        }

        public async Task<Sale> GetAsync(int id)
        {
            var sale = (await _db
                .LoadDataAsync<Sale>("dbo.spSale_GetById", new { Id = id }))
                .FirstOrDefault();

            return sale;
        }

        public async Task<IEnumerable<SaleSummary>> GetSaleSummariesAsync()
        {
            var summaries = await _db
                .LoadDataAsync<SaleSummary>("dbo.spSale_GetSummaries");

            return summaries;
        }
    }
}
