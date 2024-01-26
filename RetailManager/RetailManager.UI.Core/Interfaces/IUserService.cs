using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<IdentityUserModel>> GetUsersAsync();
    }
}
