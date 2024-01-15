using RetailManager.Core.Data.Dtos;
using RetailManager.Core.Data.Models;
using System.Threading.Tasks;

namespace RetailManager.Core.Interfaces
{
    /// <summary>
    /// A Facade to the <see cref="Sale"/> model creation subsystem of operations.
    /// </summary>
    public interface ISalePersistence
    {
        Task<Sale> Create(SaleDto saleDto, string cashierId);
    }
}