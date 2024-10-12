using Microsoft.AspNetCore.Identity;

namespace RetailManager.Api.Data.Entities
{
    public class RetailManagerAuthUser : IdentityUser
    {
        public string? RefreshToken {  get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }
}
