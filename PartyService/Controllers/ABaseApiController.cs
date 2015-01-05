using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;

namespace PartyService.Controllers
{
    public abstract class ABaseApiController : ApiController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}
