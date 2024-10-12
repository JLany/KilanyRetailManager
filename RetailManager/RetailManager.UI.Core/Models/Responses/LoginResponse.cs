using System;

namespace RetailManager.UI.Core.Models.Responses
{
    public class LoginResponse
    {
        public string Username { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
    }
}
