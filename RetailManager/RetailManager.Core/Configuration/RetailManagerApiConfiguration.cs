using RetailManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RetailManager.Core.Configuration
{
    public class RetailManagerApiConfiguration : IConfiguration
    {
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public string GetConnectionString()
        {
            return GetConnectionString(Constants.ConnectionStringName);
        }

        public decimal GetTaxRate()
        {
            var taxRate = ConfigurationManager.AppSettings["TaxRate"];

            bool isValid = decimal.TryParse(taxRate, out decimal taxRateValue);

            if (!isValid)
            {
                throw new ConfigurationErrorsException("Tax Rate is not configured properly.");
            }

            return taxRateValue;
        }
    }
}