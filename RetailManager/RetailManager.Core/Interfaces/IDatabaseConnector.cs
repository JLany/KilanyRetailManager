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
        IEnumerable<TResult> LoadData<TResult, TParams>(
            string storedProcedure,
            TParams parameters);
        void SaveData<TParams>(string storedProcedure, TParams parameters);
    }
}
