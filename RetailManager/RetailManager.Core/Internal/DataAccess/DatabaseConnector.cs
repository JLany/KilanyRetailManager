using Dapper;
using RetailManager.Core.Interfaces;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RetailManager.Core.Internal.DataAccess
{
    internal class DatabaseConnector : IDatabaseConnector
    {
        private readonly IConfiguration _configuration;

        public DatabaseConnector(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<TResult>> LoadDataAsync<TResult, TParams>(
            string storedProcedure,
            TParams parameters)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString()))
            {
                var rows = await connection
                    .QueryAsync<TResult>(storedProcedure, parameters
                    , commandType: CommandType.StoredProcedure);

                return rows;
            }
        }

        public async Task<IEnumerable<TResult>> LoadDataAsync<TResult>(
            string storedProcedure)
        {
            return await LoadDataAsync<TResult, object>(storedProcedure, new { });
        }

        public async Task SaveDataAsync<TParams>(string storedProcedure, TParams parameters)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString()))
            {
                int rowsAffected = await connection
                    .ExecuteAsync(storedProcedure, parameters
                    , commandType: CommandType.StoredProcedure);
            }
        }
    }
}
