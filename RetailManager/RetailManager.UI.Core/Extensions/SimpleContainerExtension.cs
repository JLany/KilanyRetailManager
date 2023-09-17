﻿using Caliburn.Micro;
using RetailManager.UI.Core.ApiClient;
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
                .Singleton<IApiHelper, ApiHelper>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>();

            return container;
        }
    }
}
