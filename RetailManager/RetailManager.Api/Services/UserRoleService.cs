using Microsoft.AspNetCore.Identity;
using RetailManager.Api.Data.Context;
using RetailManager.Api.Data.Entities;
using RetailManager.Api.Interfaces;
using RetailManager.Api.Models;
using RetailManager.Api.Models.Requests;
using RetailManager.Core.Data.Dtos;
using RetailManager.Core.Data.Models;

namespace RetailManager.Api.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<RetailManagerAuthUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RetailManagerAuthContext _context;

        public UserRoleService(
            UserManager<RetailManagerAuthUser> userManager,
            RoleManager<IdentityRole> roleManager,
            RetailManagerAuthContext context) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IQueryable<RoleModel> GetRoles()
        {
            return _context.Roles
                .Select(role => new RoleModel
                {
                    Id = role.Id,
                    Name = role.Name
                });
        }

        public IQueryable<UserRoleModel> GetUserRoles()
        {
            return _context.UserRoles
                .Join(
                _context.Roles,
                userRole => userRole.RoleId,
                role => role.Id,
                (userRole, role) => new UserRoleModel
                { 
                    UserId = userRole.UserId,
                    RoleId = userRole.RoleId,
                    RoleName = role.Name 
                });
        }

        public IQueryable<UserRoleModel> GetUserRoles(string userId)
        {
            var userRoles = _context.UserRoles
                .Where(userRole => userRole.UserId == userId)
                .Join(_context.Roles,
                userRole => userRole.RoleId,
                role => role.Id,
                (userRole, role) => new UserRoleModel 
                { 
                    UserId = userRole.UserId,
                    RoleId = role.Id,
                    RoleName = role.Name 
                });

            return userRoles;
        }

        public async Task AddToRoleAsync(UserRoleRequest userRole)
        {
            var user = await _userManager.FindByIdAsync(userRole.UserId);
            await _userManager.AddToRoleAsync(user, userRole.Role);
        }

        public async Task RemoveFromRoleAsync(UserRoleRequest userRole)
        {
            var user = await _userManager.FindByIdAsync(userRole.UserId);
            await _userManager.RemoveFromRoleAsync(user, userRole.Role);
        }

        public async Task InitRoles()
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
        }
    }
}
