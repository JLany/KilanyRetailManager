using RetailManager.Core.Data.Models;
using RetailManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.Core.Internal.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        private readonly IDatabaseConnector _db;

        public ProductRepository(IDatabaseConnector db) 
        {
            _db = db;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _db
                .LoadDataAsync<Product>("dbo.spProduct_GetAll");

            return products;
        }
    }
}
