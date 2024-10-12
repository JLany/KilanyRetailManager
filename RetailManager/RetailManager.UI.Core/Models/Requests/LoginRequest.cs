using System;
using System.Collections.Generic;
using System.Text;

namespace RetailManager.UI.Core.Models.Requests
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Grant_type { get; set; }
    }
}
