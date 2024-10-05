using System;

namespace RetailManager.Core.Data.Models
{
    /// <summary>
    /// This model maps to the whole DB table.
    /// </summary>
    public class InventoryBatch
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
