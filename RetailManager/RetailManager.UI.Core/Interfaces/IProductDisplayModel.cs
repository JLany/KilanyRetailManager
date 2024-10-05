namespace RetailManager.UI.Core.Interfaces
{
    public interface IProductDisplayModel
    {
        /// <summary>
        /// Global unique identifier for the product in database.
        /// </summary>
        int Id { get; set; }
        string ProductName { get; set; }
        string Description { get; set; }
        decimal RetailPrice { get; set; }
        int QuantityInStock { get; set; }
        bool IsTaxable { get; set; }
        string DisplayName { get; }
    }
}
