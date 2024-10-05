using Autofac;
using Autofac.Integration.WebApi;
using RetailManager.Core.AutofacModules;
using System.Reflection;
using System.Web.Http;

namespace RetailManager.Api.DependencyInjection
{
    public static class AutofacConfig
    {
        public static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            HttpConfiguration config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterWebApiFilterProvider(config);

            // Add RetailManager.Core Services.
            builder.RegisterModule<RetailManagerCoreModule>();

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return container;
        }
    }
}