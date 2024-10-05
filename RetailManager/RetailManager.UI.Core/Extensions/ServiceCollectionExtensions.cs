using Microsoft.Extensions.DependencyInjection;
using RetailManager.UI.Core.ApiClients;
using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;

namespace RetailManager.UI.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRetailManagerUiCore(this IServiceCollection services)
        {
            services
                .AddSingleton<IApiClient, ApiClient>()
                .AddSingleton<IUserPrincipal, UserPrincipal>()
                .AddSingleton<RetailManager.UI.Core.Interfaces.IConfiguration, RetailManagerUIConfiguration>()

                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<ISaleService, SaleService>()
                .AddScoped<IAdminService, AdminService>();

            return services;
        }
    }
}
