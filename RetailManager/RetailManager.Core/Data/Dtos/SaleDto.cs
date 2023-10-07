using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.Core.Data.Dtos
{
    public class SaleDto
    {
        public IEnumerable<SaleDetailDto> SaleDetails;
    }
}
