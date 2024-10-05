using System.Collections.Generic;

namespace RetailManager.Core.Data.Models
{
    public class UserRolesViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public IEnumerable<SimpleRoleModel> Roles { get; set; } = new List<SimpleRoleModel>();
    }
}
