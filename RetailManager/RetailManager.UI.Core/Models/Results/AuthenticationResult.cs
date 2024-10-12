using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RetailManager.UI.Core.Models.Results
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
