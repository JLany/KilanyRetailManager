using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RetailManager.Api.Configuration;
using RetailManager.Api.Data.Context;
using RetailManager.Api.Data.Entities;
using RetailManager.Api.Extensions;
using RetailManager.Core.Extensions;

namespace RetailManager.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString(Constants.RetailManagerAuthContextConnectionStringKey)
                ?? throw new InvalidOperationException($"Connection string {Constants.RetailManagerAuthContextConnectionStringKey} not found.");

            builder.Services.AddDbContext<RetailManagerAuthContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentityCore<RetailManagerAuthUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<RetailManagerAuthContext>()
                .AddSignInManager();

            var jwtSettings = builder.Configuration.GetSection(Constants.JwtSettingsSectionKey).Get<JwtSettings>();
            builder.Services.AddJwtBearerAuthentication(jwtSettings);

            builder.Services.AddAuthorization();

            builder.Services.AddControllersWithViews();

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(Constants.JwtSettingsSectionKey));

            builder.Services.AddRetailManagerCore()
                .AddServices();

            builder.Services.AddRazorPages();

            builder.Services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Title = "Kilany Retail Manager API",
                    Version = "v1.0"
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Kilany Retail Manager API");
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}