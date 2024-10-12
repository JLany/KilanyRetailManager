using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RetailManager.Api.Data.Entities;

namespace RetailManager.Api.Data.Context
{
    public class RetailManagerAuthContext : IdentityDbContext<RetailManagerAuthUser>
    {
        public RetailManagerAuthContext(DbContextOptions<RetailManagerAuthContext> options)
            : base(options) { }
    }
}
