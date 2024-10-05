using RetailManager.Core.Data.Models;
using System.Threading.Tasks;

namespace RetailManager.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
    }
}
