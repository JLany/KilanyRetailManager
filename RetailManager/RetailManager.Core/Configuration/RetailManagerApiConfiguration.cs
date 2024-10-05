using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace RetailManager.Core.Configuration
{
    public class RetailManagerApiConfiguration : Interfaces.IConfiguration
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;

        public RetailManagerApiConfiguration(Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _config = config;
        }

        public string GetConnectionString(string name)
        {
            return _config.GetConnectionString(name);
        }

        public string GetConnectionString()
        {
            return GetConnectionString(Constants.ConnectionStringName);
        }

        public decimal GetTaxRate()
        {
            var taxRate = _config["TaxRate"];

            bool isValid = decimal.TryParse(taxRate, out decimal taxRateValue);

            if (!isValid)
            {
                throw new ConfigurationErrorsException("Tax Rate is not configured properly.");
            }

            return taxRateValue;
        }
    }
}