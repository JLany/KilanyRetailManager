using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.Core.Data.Models
{
    public class UserRolesViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public IEnumerable<SimpleRoleModel> Roles { get; set; } = new List<SimpleRoleModel>();
    }
}
