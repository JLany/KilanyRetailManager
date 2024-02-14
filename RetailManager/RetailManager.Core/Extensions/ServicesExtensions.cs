using Microsoft.Extensions.DependencyInjection;
using RetailManager.Core.Configuration;
using RetailManager.Core.Interfaces;
using RetailManager.Core.Internal.DataAccess;
using RetailManager.Core.Internal.Repositories;

namespace RetailManager.Core.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddRetailManagerCore(this IServiceCollection services)
        {
            return services
                .AddScoped<IInventoryBatchRepository, InventoryBatchRepository>()
                .AddScoped<IDatabaseConnector, DatabaseConnector>()
                .AddScoped<IProductRepository, ProductRepository>()
                .AddScoped<ISaleRepository, SaleRepository>()
                .AddScoped<ISaleDetailRepository, SaleDetailRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>()

                .AddTransient<IConfiguration, RetailManagerApiConfiguration>();
        }
    }
}
