using Microsoft.AspNet.Identity;
using RetailManager.Core.Data.Models;
using RetailManager.Core.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace RetailManager.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private readonly IUserRepository _userRepo;
        
        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
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
    }
}