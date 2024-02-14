using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RetailManager.Api.Models;
using RetailManager.Core.Data.Dtos;
using RetailManager.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RetailManager.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/Admin")]
    public class AdminController : ApiController
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                // TODO: After upgrading to .NET Core use this ??=
                return _userManager ?? (UserManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>());
            }
            private set { _userManager = value; }
        }

        [HttpGet]
        [Route("UserList")]
        public IHttpActionResult GetAllUsers()
        {
            IEnumerable<UserRolesViewModel> users;

            using (var context = new ApplicationDbContext())
            {
                var contextRoles = context.Roles.ToList();
                var contextUsers = UserManager.Users.ToList();

                users = contextUsers.Select(u => new UserRolesViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    Roles = contextRoles
                        .Where(role => u.Roles.Any(r => r.RoleId == role.Id))
                        .Select(role => new SimpleRoleModel 
                        { 
                            Id = role.Id,
                            Name = role.Name 
                        })
                });
            }

            return Ok(users);
        }

        [HttpGet]
        [Route("RoleList")]
        public IHttpActionResult GetAllRoles()
        {
            using (var context = new ApplicationDbContext())
            {
                var contextRoles = context.Roles.ToList();

                return Ok(contextRoles
                    .Select(role => new SimpleRoleModel
                    {
                        Id = role.Id,
                        Name = role.Name
                    })
                    );
            }
        }

        [HttpPost]
        [Route("AddToRole")]
        public IHttpActionResult AddToRole(UserRoleDto userRole)
        {
            UserManager.AddToRole(userRole.UserId, userRole.Role);

            return Ok();
        }

        [HttpPost]
        [Route("RemoveFromRole")]
        public IHttpActionResult RemoveFromRole(UserRoleDto userRole)
        {
            UserManager.RemoveFromRole(userRole.UserId, userRole.Role);

            return Ok();
        }
    }
}
