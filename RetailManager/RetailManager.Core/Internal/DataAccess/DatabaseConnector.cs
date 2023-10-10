using Dapper;
using RetailManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RetailManager.Core.Internal.DataAccess
{
    internal class DatabaseConnector : IDatabaseConnector
    {
        private readonly IConfiguration _config;

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public DatabaseConnector(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<TResult>> LoadDataAsync<TResult>(
            string storedProcedure,
            object parameters)
        {
            BeginConnection();

            var rows = await _connection
                .QueryAsync<TResult>(storedProcedure, parameters
                , commandType: CommandType.StoredProcedure, transaction: _transaction);

            EndConnection();

            return rows;
        }

        public async Task<IEnumerable<TResult>> LoadDataAsync<TResult>(
            string storedProcedure)
        {
            return await LoadDataAsync<TResult>(storedProcedure, new { });
        }

        public async Task SaveDataAsync(string storedProcedure, object parameters)
        {
            BeginConnection();

            int rowsAffected = await _connection
                .ExecuteAsync(storedProcedure, parameters
                , commandType: CommandType.StoredProcedure, transaction: _transaction);
            
            EndConnection();
        }

        public async Task<TOutputParameter> SaveDataAsync<TOutputParameter>(string storedProcedure, object parameters)
        {
            BeginConnection();

            DynamicParameters p = new DynamicParameters();
            p.AddDynamicParams(parameters);
            p.Add("@Id", null, DbType.Int32, ParameterDirection.Output);

            int rowsAffected = await _connection
                .ExecuteAsync(storedProcedure, p
                , commandType: CommandType.StoredProcedure, transaction: _transaction);

            EndConnection();

            return p.Get<TOutputParameter>(@"Id");
        }

        public void BeginTransaction()
        {
            BeginConnection();

            _transaction = _connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();

            _transaction?.Dispose();
            _transaction = null;

            EndConnection();
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();

            _transaction?.Dispose();
            _transaction = null;

            EndConnection();
        }

        private void BeginConnection()
        {
            if (_connection?.State == ConnectionState.Open)
            {
                return;
            }

            _connection = new SqlConnection(_config.GetConnectionString());
            _connection.Open();
        }

        private void EndConnection()
        {
            if (_transaction == null)
            {
                _connection?.Dispose();
                _connection = null;
            }
        }

        public void Dispose()
        {
            CommitTransaction();
        }
    }
}
