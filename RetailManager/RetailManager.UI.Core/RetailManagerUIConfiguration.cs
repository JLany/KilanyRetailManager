using RetailManager.UI.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core
{
    public class RetailManagerUIConfiguration : IConfiguration
    {
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

        public string GetApiBaseAddress()
        {
            string baseAddress = ConfigurationManager.AppSettings["ApiBaseAddress"]
               ?? throw new ConfigurationErrorsException("Setting 'ApiBaseAddress' was not found.");

            return baseAddress;
        }
    }
}
