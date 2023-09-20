using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Models
{
    public class UserPrincipal : IUserPrincipal
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
