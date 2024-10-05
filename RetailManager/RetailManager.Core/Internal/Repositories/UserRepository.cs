using RetailManager.Core.Data.Models;
using RetailManager.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace RetailManager.Core.Internal.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly IDatabaseConnector _db;

        public UserRepository(IDatabaseConnector db)
        {
            _db = db;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            User user = (await _db
                .LoadDataAsync<User>("dbo.spUser_GetById", new { Id = id }))
                .FirstOrDefault();

            return user;
        }
    }
}
