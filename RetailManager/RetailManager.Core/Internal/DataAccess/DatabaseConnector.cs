using Dapper;
using RetailManager.Core.Interfaces;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace RetailManager.Core.Internal.DataAccess
{
    internal class DatabaseConnector : IDatabaseConnector
    {
        private readonly IConfiguration _configuration;

        public DatabaseConnector(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<TResult> LoadData<TResult, TParams>(
            string storedProcedure,
            TParams parameters)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString()))
            {
                var rows = connection
                    .Query<TResult>(storedProcedure, parameters
                    , commandType: CommandType.StoredProcedure);

                return rows;
            }
        }

        public void SaveData<TParams>(string storedProcedure, TParams parameters)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString()))
            {
                int rowsAffected = connection
                    .Execute(storedProcedure, parameters
                    , commandType: CommandType.StoredProcedure);
            }
        }
    }
}
