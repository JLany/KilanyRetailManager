﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(RetailManager.Api.Startup))]

namespace RetailManager.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
