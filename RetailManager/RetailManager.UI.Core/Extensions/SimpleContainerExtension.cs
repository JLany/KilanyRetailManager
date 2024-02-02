using Caliburn.Micro;
using RetailManager.UI.Core.ApiClients;
using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Extensions
{
    public static class SimpleContainerExtension
    {
        public static SimpleContainer AddRetailManagerUiCore(this SimpleContainer container)
        {
            container
                .Singleton<IApiClient, ApiClient>()
                .Singleton<IUserPrincipal, UserPrincipal>()
                .Singleton<RetailManager.UI.Core.Interfaces.IConfiguration, RetailManagerUIConfiguration>()

                .PerRequest<IAuthenticationService, AuthenticationService>()
                .PerRequest<IProductService, ProductService>()
                .PerRequest<ISaleService, SaleService>()
                .PerRequest<IAdminService, AdminService>();

            return container;
        }
    }
}
