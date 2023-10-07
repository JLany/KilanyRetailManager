using Autofac;
using Autofac.Integration.WebApi;
using RetailManager.Core.AutofacModules;
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Configuration.ConfigurationBuilders;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
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