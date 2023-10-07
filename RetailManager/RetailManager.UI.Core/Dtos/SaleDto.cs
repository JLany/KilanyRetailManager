using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Dtos
{
    public class SaleDto
    {
        public IEnumerable<SaleDetailDto> SaleDetails { get; set; } = new HashSet<SaleDetailDto>();
    }
}
