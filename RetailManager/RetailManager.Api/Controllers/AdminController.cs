using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RetailManager.Api.Data;
using RetailManager.Api.Data.Context;
using RetailManager.Api.Data.Entities;
using RetailManager.Api.Interfaces;
using RetailManager.Api.Models.Requests;
using RetailManager.Core.Data.Dtos;
using RetailManager.Core.Data.Models;

namespace RetailManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<RetailManagerAuthUser> _userManager;
        private readonly IUserRoleService _userRoles;

        public AdminController(
            UserManager<RetailManagerAuthUser> userManager,
            RoleManager<IdentityRole> roleManager,
            RetailManagerAuthContext context,
            IUserRoleService userRoles)
        {
            _userManager = userManager;
            _userRoles = userRoles;
        }

        [HttpGet]
        [Route("UserList")]
        public IActionResult GetAllUsers()
        {
            IEnumerable<UserRolesViewModel> users;

            var contextUsers = _userManager.Users.ToList();
            var userRoles = _userRoles.GetUserRoles();

            users = contextUsers.Select(user => new UserRolesViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Roles = userRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Select(role => new SimpleRoleModel
                    {
                        Id = role.RoleId,
                        Name = role.RoleName
                    })
            });

            return Ok(users);
        }

        [HttpGet]
        [Route("RoleList")]
        public IActionResult GetAllRoles()
        {
            var contextRoles = _userRoles.GetRoles().ToList();

            return Ok(contextRoles
                .Select(role => new SimpleRoleModel
                {
                    Id = role.Id,
                    Name = role.Name
                })
            );
        }

        [HttpPost]
        [Route("AddToRole")]
        public async Task<IActionResult> AddToRole(UserRoleRequest userRole)
        {
            await _userRoles.AddToRoleAsync(userRole);

            return Ok();
        }

        [HttpPost]
        [Route("RemoveFromRole")]
        public async Task<IActionResult> RemoveFromRole(UserRoleRequest userRole)
        {
            await _userRoles.RemoveFromRoleAsync(userRole);

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("InitRoles")]
        public async Task<IActionResult> InitRoles()
        {
            await _userRoles.InitRoles();

            return Ok();
        }
    }
}
