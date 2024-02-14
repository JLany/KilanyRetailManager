using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.Core.Data.Models
{
    public class RetailManagerUser
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public IEnumerable<SimpleRoleModel> Roles { get; set; } = new List<SimpleRoleModel>();
    }
}
