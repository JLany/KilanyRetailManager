using AutoMapper;
using Caliburn.Micro;
using RetailManager.DesktopUI.Models;
using RetailManager.UI.Core.Models;

namespace RetailManager.DesktopUI.Extensions
{
    public static class SimpleContainerExtensions
    {
        public static void ConfigureAutoMapper(this SimpleContainer container)
        {
            IMapper mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<ListedProductViewModel, ListedProductDisplayModel>();
                config.CreateMap<CartItemModel, CartItemDisplayModel>();
                config.CreateMap<UserModel, UserDisplayModel>();
            })
                .CreateMapper();

            container.Instance(mapper);
        }
    }
}
