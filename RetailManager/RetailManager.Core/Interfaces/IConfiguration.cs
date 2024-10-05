namespace RetailManager.Core.Interfaces
{
    public interface IConfiguration
    {
        string GetConnectionString(string name);
        string GetConnectionString();
        decimal GetTaxRate();
    }
}
