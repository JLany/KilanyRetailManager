using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace RetailManager.Core.Internal.DataAccess
{
    internal class DatabaseConnection
    {
        private readonly string _connectionString = GetConnectionString("KilanyRetailManagementDB");

        // TODO: Refactor this method into a config type, and register it with DI.
        private static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public IEnumerable<TResult> LoadData<TResult, TParams>(
            string storedProcedure,
            TParams parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var rows = connection
                    .Query<TResult>(storedProcedure, parameters
                    , commandType: CommandType.StoredProcedure);

                return rows;
            }
        }

        public void SaveData<TParams>(string storedProcedure, TParams parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                int rowsAffected = connection
                    .Execute(storedProcedure, parameters
                    , commandType: CommandType.StoredProcedure);
            }
        }
    }
}
