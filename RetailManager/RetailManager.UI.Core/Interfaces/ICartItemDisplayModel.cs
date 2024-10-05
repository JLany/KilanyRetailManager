namespace RetailManager.UI.Core.Interfaces
{
    public interface ICartItemDisplayModel
    {
        IProductDisplayModel Product { get; set; }
        int QuantityInCart { get; set; }
        string DisplayName { get; }
    }
}
