using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RetailManager.Api.Utils.Jwt;

namespace RetailManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;

        public TokenController(
            UserManager<IdentityUser> userManager,
            ITokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] string username, [FromForm] string password, [FromForm] string grant_type)
        {
            var user = await _userManager.FindByEmailAsync(username);
            bool validCredentials = await _userManager.CheckPasswordAsync(user, password);

            if (!validCredentials)
            {
                return BadRequest();
            }

            return new ObjectResult(await _tokenGenerator.GenerateToken(username));
        }
    }
}
