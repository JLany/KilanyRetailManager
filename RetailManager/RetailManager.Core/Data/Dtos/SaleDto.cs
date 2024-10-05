using System.Collections.Generic;

namespace RetailManager.Core.Data.Dtos
{
    public class SaleDto
    {
        public List<SaleDetailDto> SaleDetails { get; set; } = new List<SaleDetailDto>();
    }
}
