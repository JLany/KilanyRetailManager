using System;

namespace RetailManager.Core.Data.Dtos
{
    public class InventoryBatchDto
    {
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
