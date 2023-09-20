using Microsoft.AspNet.Identity;
using RetailManager.Core.Data.Models;
using RetailManager.Core.Interfaces;
using System.Web.Http;

namespace RetailManager.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/User")]
    public class UsersController : ApiController
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet]
        public IHttpActionResult Info()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            User user = _userRepo.GetById(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}