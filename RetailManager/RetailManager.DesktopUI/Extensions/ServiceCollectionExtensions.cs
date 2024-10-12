using Caliburn.Micro;
using Microsoft.Extensions.DependencyInjection;
using RetailManager.DesktopUI.Models;
using RetailManager.UI.Core.Models;
using RetailManager.UI.Core.Models.Responses;

namespace RetailManager.DesktopUI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMVVM(this IServiceCollection services, Bootstrapper bootstrapper)
        {
            bootstrapper.GetType()
                .Assembly
                .GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType
                    => services.AddTransient(
                        viewModelType, viewModelType));

            services
                .AddSingleton<IWindowManager, WindowManager>()
                .AddSingleton<IEventAggregator, EventAggregator>();

            return services;
        }

        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                config.CreateMap<ListedProductViewModel, ListedProductDisplayModel>();
                config.CreateMap<CartItemModel, CartItemDisplayModel>();
                config.CreateMap<UserModel, UserDisplayModel>();
                config.CreateMap<ProductResponse, ListedProductViewModel>();
                config.CreateMap<ProductResponse, ListedProductDisplayModel>();
            });

            return services;
        }
    }
}
