using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RetailManager.Api.Models;
using RetailManager.Core.Data.Models;
using RetailManager.Core.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RetailManager.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private ApplicationUserManager _userManager;
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set { _userManager = value; }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Info()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            User user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Admin/UserList")]
        public IHttpActionResult GetAllUsers()
        {
            IEnumerable<IdentityUser> users;

            using (var context = new ApplicationDbContext())
            {
                var contextRoles = context.Roles.ToList();
                var contextUsers = UserManager.Users.ToList();

                users = contextUsers.Select(u => new IdentityUser
                {
                    Id = u.Id,
                    Email = u.Email,
                    Roles = contextRoles.Where(role => u.Roles.Any(r => r.RoleId == role.Id))
                        .ToDictionary(role => role.Id, role => role.Name)
                });
            }

            return Ok(users);
        }
    }
}