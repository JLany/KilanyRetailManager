using Microsoft.AspNet.Identity;
using RetailManager.Core.Data.Models;
using RetailManager.Core.Data.Repositories;
using System.Web.Http;

namespace RetailManager.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/User")]
    public class UsersController : ApiController
    {
        private readonly UserRepository _userRepo;

        public UsersController()
        {
            _userRepo = new UserRepository();
        }

        public IHttpActionResult GetById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            UserModel user = _userRepo.GetById(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}