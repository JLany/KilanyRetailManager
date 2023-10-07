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

        public async Task<IEnumerable<TResult>> LoadDataAsync<TResult>(
            string storedProcedure,
            object parameters)
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
            return await LoadDataAsync<TResult>(storedProcedure, new { });
        }

        public async Task SaveDataAsync(string storedProcedure, object parameters)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString()))
            {
                int rowsAffected = await connection
                    .ExecuteAsync(storedProcedure, parameters
                    , commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<TOutputParameter> SaveDataAsync<TOutputParameter>(string storedProcedure, object parameters)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString()))
            {
                DynamicParameters p = new DynamicParameters();
                p.AddDynamicParams(parameters);
                p.Add("@Id", null, DbType.Int32, ParameterDirection.Output);

                int rowsAffected = await connection
                    .ExecuteAsync(storedProcedure, p
                    , commandType: CommandType.StoredProcedure);

                return p.Get<TOutputParameter>(@"Id");
            }
        }
    }
}
