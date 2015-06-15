using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Sandbox.Models;

namespace Sandbox.Controllers
{
    /// <summary>
    /// The Account Test Controller allows you to test your client credentials against the server.
    /// </summary>
    [Authorize]
    public class AccountTestController : ApiController
    {
        private ApplicationUserManager _userManager;

        /// <summary>
        /// Gets or sets the user manager.
        /// </summary>
        /// <value>
        /// The user manager.
        /// </value>
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            protected set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// Once your are authenticated this method Gets your user name, account number, and API key data for your login.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest("err: not authenticated");
            }
            var userId = User.Identity.GetUserId();
            var appUser = await UserManager.FindByIdAsync(userId);
            return Ok(new { UserName = appUser.UserName, AccountNumber = appUser.AccountNumber, ApiKey = appUser.ApiKey });
        }
    }
}