using System;
using System.Collections.Generic;
using System.Text;

namespace RetailManager.UI.Core.Models.Requests
{
    public class RefreshTokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
