using System.Collections.Generic;

namespace RetailManager.Core.Data.Dtos
{
    public class SaleRequest
    {
        public List<SaleDetailRequest> SaleDetails { get; set; } = new List<SaleDetailRequest>();
    }
}
