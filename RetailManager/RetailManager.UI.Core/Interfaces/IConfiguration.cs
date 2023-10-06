using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Interfaces
{
    public interface IConfiguration
    {
        string GetApiBaseAddress();
        decimal GetTaxRate();
    }
}
