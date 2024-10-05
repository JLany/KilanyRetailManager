using System;

namespace RetailManager.Core.Data.Models
{
    public class SaleSummary
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string CashierFirstName { get; set; }
        public string CashierLastName { get; set; }
        public string CashierEmailAddress { get; set; }
    }
}
