using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RetailManager.Api.Configuration
{
    public class RetailManagerApiConfiguration : RetailManager.Core.Interfaces.IConfiguration
    {
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public string GetConnectionString()
        {
            return GetConnectionString(Constants.ConnectionStringName);
        }
    }
}