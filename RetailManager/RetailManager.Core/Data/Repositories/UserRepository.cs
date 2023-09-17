using RetailManager.Core.Data.Models;
using RetailManager.Core.Internal.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.Core.Data.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public UserRepository()
        {
            // TODO: Use DI.
            _dbConnection = new DatabaseConnection();
        }

        public UserModel GetById(string id)
        {
            UserModel user = _dbConnection
                .LoadData<UserModel, object>("dbo.spUser_GetById", new { Id = id  })
                .FirstOrDefault();

            return user;
        } 
    }

}
