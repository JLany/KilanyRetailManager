using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RetailManager.Api.Data;
using RetailManager.Core.Data.Dtos;
using RetailManager.Core.Data.Models;

namespace RetailManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        [Route("UserList")]
        public IActionResult GetAllUsers()
        {
            IEnumerable<UserRolesViewModel> users;

            var contextUsers = _userManager.Users.ToList();
            var userRoles = _context.UserRoles
                .Join(
                _context.Roles,
                userRole => userRole.RoleId,
                role => role.Id,
                (userRole, role) => new { userRole.UserId, userRole.RoleId, role.Name }
                );

            users = contextUsers.Select(user => new UserRolesViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Roles = userRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Select(role => new SimpleRoleModel
                    {
                        Id = role.RoleId,
                        Name = role.Name
                    })
            });

            return Ok(users);
        }

        [HttpGet]
        [Route("RoleList")]
        public IActionResult GetAllRoles()
        {
            var contextRoles = _context.Roles.ToList();

            return Ok(contextRoles
                .Select(role => new SimpleRoleModel
                {
                    Id = role.Id,
                    Name = role.Name
                })
            );
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(UserRoleDto userRole)
        {
            var user = await _userManager.FindByIdAsync(userRole.UserId);
            await _userManager.AddToRoleAsync(user, userRole.Role);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(UserRoleDto userRole)
        {
            var user = await _userManager.FindByIdAsync(userRole.UserId);
            await _userManager.RemoveFromRoleAsync(user, userRole.Role);

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("InitRoles")]
        public async Task<IActionResult> InitRoles()
        {
            string[] roles = { "Admin", "Manager", "Cashier" };

            foreach (var role in roles)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var admin = await _userManager.FindByEmailAsync("test@test.test");

            if (admin != null)
            {
                await _userManager.AddToRoleAsync(admin, "Admin");
                await _userManager.AddToRoleAsync(admin, "Cashier");
            }

            return Ok();
        }
    }
}
