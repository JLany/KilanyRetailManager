using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using RetailManager.UI.Core.Services;
using RetailManager.UI.Core.Clients;
using RetailManager.UI.Core.Configuration;
using RetailManager.UI.Core.DelegatingHandlers;
using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System;
using System.Configuration;

namespace RetailManager.UI.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRetailManagerUiCore(this IServiceCollection services)
        {
            services.AddServices();

            var config = Infrastructure.InitConfiguration().Build();
            var apiSettings = config.GetSection(Constants.ApiSettingsKey).Get<ApiSettings>();

            services.Configure<ApiSettings>(config.GetSection(Constants.ApiSettingsKey));
            services.Configure<BusinessSettings>(config.GetSection(Constants.BusinessSettingsKey));


            services.AddTransient<AuthenticationDelegatingHandler>();
            services
                .AddRefitClient<IUserClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiSettings.ApiBaseAddress))
                .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

            services
                .AddRefitClient<IAuthenticationClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiSettings.ApiBaseAddress));

            services
                .AddRefitClient<IProductClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiSettings.ApiBaseAddress))
                .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

            services
                .AddRefitClient<ISaleClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiSettings.ApiBaseAddress))
                .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

            services
                .AddRefitClient<IAdminClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiSettings.ApiBaseAddress))
                .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IApiClient, ApiClient>()
                .AddSingleton<IUserPrincipal, UserPrincipal>()
                .AddSingleton<RetailManager.UI.Core.Interfaces.IConfiguration, RetailManagerUIConfiguration>()

                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<ISaleService, SaleService>()
                .AddScoped<IAdminService, AdminService>()
                .AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
