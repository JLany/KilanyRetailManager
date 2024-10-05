
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RetailManager.Api.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RetailManager.Api.Utils.Jwt
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public JwtTokenGenerator(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<object?> GenerateToken(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            // No check for null here because we checked credentials already
            // before trying to generate a token.

            var userRoles = _context.UserRoles
                .Where(userRole => userRole.UserId == user.Id)
                .Join(_context.Roles,
                userRole => userRole.RoleId,
                role => role.Id,
                (userRole, role) => new { userRole.UserId, role.Id, role.Name });

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, username),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddHours(2)).ToUnixTimeSeconds().ToString())
            };

            await userRoles.ForEachAsync(role =>
            {
                claims.Add(new(ClaimTypes.Role, role.Name));
            });

            // TODO: Remove the key from the source code. Put in configuration or something.
            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecureAndSecretKeyForDebuggingPLEASECHANGE")),
                        SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            var authentication = new
            {
                Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = username
            };

            return authentication;
        }
    }
}
