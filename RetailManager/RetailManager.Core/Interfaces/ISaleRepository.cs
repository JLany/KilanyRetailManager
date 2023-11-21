using RetailManager.Core.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailManager.Core.Interfaces
{
    public interface ISaleRepository
    {
        Task AddAsync(Sale sale);
        Task<Sale> GetAsync(int id);
        Task<IEnumerable<SaleSummary>> GetSaleSummariesAsync();

        /// <summary>
        /// If the <see cref="Sale"/> is not in the database, create it.
        /// </summary>
        /// <param name="sale"></param>
        /// <returns></returns>
        Task UpdateAsync(Sale sale);
    }
}