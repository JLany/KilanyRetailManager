using RetailManager.Core.Data.Models;
using RetailManager.Core.Interfaces;
using RetailManager.Core.Internal.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.Core.Internal.Data.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly IDatabaseConnector _dbConnection;

        public UserRepository(IDatabaseConnector dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public UserModel GetById(string id)
        {
            UserModel user = _dbConnection
                .LoadData<UserModel, object>("dbo.spUser_GetById", new { Id = id })
                .FirstOrDefault();

            return user;
        }
    }

}
