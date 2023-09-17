using Autofac;
using RetailManager.Core.Interfaces;
using RetailManager.Core.Internal.Data.Repositories;
using RetailManager.Core.Internal.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerRequest();
        }
    }
}
