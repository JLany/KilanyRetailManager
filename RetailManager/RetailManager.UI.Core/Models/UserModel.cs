using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public List<RoleModel> Roles { get; set; }

        public string RolesList => string.Join(", ", Roles.Select(r => r.Name));
    }
}
