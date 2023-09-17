using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.Core.Interfaces
{
    public interface IConfiguration
    {
        string GetConnectionString(string name);
        string GetConnectionString();
    }
}
