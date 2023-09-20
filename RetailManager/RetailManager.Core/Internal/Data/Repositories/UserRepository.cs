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

        public User GetById(string id)
        {
            User user = _dbConnection
                .LoadData<User, object>("dbo.spUser_GetById", new { Id = id })
                .FirstOrDefault();

            return user;
        }
    }

}
