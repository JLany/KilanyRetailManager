using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RetailManager.Core.Data.Models;
using RetailManager.Core.Interfaces;

namespace RetailManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(IUserRepository userRepo, UserManager<IdentityUser> userManager)
        {
            _userRepo = userRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Info()
        {
            string userId = _userManager.GetUserId(User);
            User user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}