using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RetailManager.Api.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RetailManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public TokenController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] string username, [FromForm] string password, [FromForm] string grant_type)
        {
            bool validCredentials = await IsValidCredentials(username, password);

            if (!validCredentials)
            {
                return BadRequest();
            }

            return new ObjectResult(await GenerateToken(username));
        }

        private async Task<object?> GenerateToken(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            // No check for null here because we checked credentials already.

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

        private async Task<bool> IsValidCredentials(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);

            return await _userManager.CheckPasswordAsync(user, password);
        }


    }
}
