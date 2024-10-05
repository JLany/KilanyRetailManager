namespace RetailManager.UI.Core.Interfaces
{
    public interface IConfiguration
    {
        string GetApiBaseAddress();
        decimal GetTaxRate();
    }
}
