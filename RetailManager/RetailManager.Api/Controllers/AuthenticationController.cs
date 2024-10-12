using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailManager.Api.Exceptions;
using RetailManager.Api.Interfaces;
using RetailManager.Api.Models.Requests;

namespace RetailManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthenticationController(
            IAuthService auth)
        {
            _auth = auth;
        }

        [Route("Login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            try
            {
                var response = await _auth.LoginAsync(login);

                return Ok(response);
            }
            catch(UnauthorizedException)
            {
                return Unauthorized();
            }
        }

        [Route("Refresh")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var response = await _auth.RefreshTokenAsync(request);
                return Ok(response);
            }
            catch(UnauthorizedException)
            {
                return Unauthorized();
            }
        }

        [Route("Logout")]
        [HttpDelete]
        public async Task<IActionResult> Logout()
        {
            var username = HttpContext.User.Identity?.Name;

            try
            {
                await _auth.LogoutAsync(username);
                return Ok();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
        }
    }
}
