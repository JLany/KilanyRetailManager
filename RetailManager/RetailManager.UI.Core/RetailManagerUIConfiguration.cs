using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace RetailManager.UI.Core
{
    public class RetailManagerUIConfiguration : Interfaces.IConfiguration
    {
        private readonly IConfiguration _config;

        public RetailManagerUIConfiguration(IConfiguration config)
        {
            _config = config;
        }

        // TODO: Move this to the API instead of the configuration, and pull it out from there.
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

        public string GetApiBaseAddress()
        {
            string baseAddress = _config["ApiBaseAddress"]
               ?? throw new ConfigurationErrorsException("Setting 'ApiBaseAddress' was not found.");

            return baseAddress;
        }
    }
}
