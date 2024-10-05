using System.Collections.Generic;
using System.Linq;

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
