using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.Core.Interfaces
{
    internal interface IDatabaseConnector
    {
        Task<IEnumerable<TResult>> LoadDataAsync<TResult, TParams>(
            string storedProcedure,
            TParams parameters);
        Task<IEnumerable<TResult>> LoadDataAsync<TResult>(string storedProcedure);
        Task SaveDataAsync<TParams>(string storedProcedure, TParams parameters);
    }
}
