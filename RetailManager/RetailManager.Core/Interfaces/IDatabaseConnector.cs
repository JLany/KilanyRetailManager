using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.Core.Interfaces
{
    internal interface  IDatabaseConnector : IDisposable
    {
        void BeginTransaction();
        void CommitTransaction();
        Task<IEnumerable<TResult>> LoadDataAsync<TResult>(
            string storedProcedure,
            object parameters);
        Task<IEnumerable<TResult>> LoadDataAsync<TResult>(string storedProcedure);
        void RollbackTransaction();
        Task SaveDataAsync(string storedProcedure, object parameters);

        /// <summary>
        /// Save to database, and return the Id.
        /// </summary>
        /// <typeparam name="TOutputParameter"></typeparam>
        /// <param name="storedProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<TOutputParameter> SaveDataAsync<TOutputParameter>(string storedProcedure, object parameters);
    }
}
