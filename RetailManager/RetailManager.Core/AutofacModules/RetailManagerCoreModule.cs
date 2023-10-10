using Autofac;
using RetailManager.Core.Interfaces;
using RetailManager.Core.Internal.Repositories;
using RetailManager.Core.Internal.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetailManager.Core.Configuration;

namespace RetailManager.Core.AutofacModules
{
    public class RetailManagerCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<DatabaseConnector>()
                .As<IDatabaseConnector>()
                .InstancePerRequest();

            builder
                .RegisterType<RetailManagerApiConfiguration>()
                .As<IConfiguration>()
                .InstancePerRequest();

            builder
                .RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerRequest();
            
            builder
                .RegisterType<ProductRepository>()
                .As<IProductRepository>()
                .InstancePerRequest();

            builder
                .RegisterType<SaleRepository>()
                .As<ISaleRepository>()
                .InstancePerRequest();

            builder
                .RegisterType<SaleDetailRepository>()
                .As<ISaleDetailRepository>()
                .InstancePerRequest();

            builder
                .RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();
        }
    }
}
