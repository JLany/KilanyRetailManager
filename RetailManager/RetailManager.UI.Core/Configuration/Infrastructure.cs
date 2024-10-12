using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;

namespace RetailManager.UI.Core.Configuration
{
    public static class Infrastructure
    {
        public static IConfigurationBuilder InitConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json"
                , optional: true)
                .AddEnvironmentVariables();

            return builder;
        }

        public static void ConfigureLogger(IConfigurationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Bootstrapping...");
        }
    }
}
