using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PartyService.ControllerModels;
using PartyService.ControllerModels.App;
using PartyService.Models;
using PartyService.Providers;

namespace PartyService.Controllers
{
    [Authorize] //(Roles = Roles.Admin)] does not work at the moment
    public class UserController : ABaseApiController
    {
        // GET: api/User
        public async Task<IHttpActionResult> Get()
        {
            if ( ! await IsAdmin() )
                return new NoAuthorized();

            var details = await UserProviderFactory
                .Create( UserManager)
                .GetAllWebUserDetailsAsync();

            return Ok(details);
        }

        // GET: api/User/8453...
        public async Task<IHttpActionResult> Get(string id)
        {
            if (!await IsAdmin())
                return new NoAuthorized();

            WebUserDetail detail = await UserProviderFactory
                .Create(UserManager)
                .GetWebUserDetailAsync(id);

            if ( detail == null )
                return NotFound();

            return Ok( detail );
        }

        
        // POST: api/User
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]AddUser model)
        {
            if (!await IsAdmin())
                return new NoAuthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await UserProviderFactory.Create(UserManager ).AdminAddUserAsync( model );

            if ( !result.Succeeded )
                return BadRequest(result.ErrorMessage);

            return Ok( result.Result );
        }

        
        // PUT: api/User/5
        [HttpPut]
        public async Task<IHttpActionResult> Put(string id, [FromBody]ChangeUser user)
        {
            if (!await IsAdmin())
                return new NoAuthorized();

            if ( !ModelState.IsValid )
                return BadRequest( ModelState );

            if ( ( await UserManager.FindByIdAsync( id ) ) == null )
                NotFound();

            var result = await UserProviderFactory.Create( UserManager ).AdminChangeUserAsync( id, user );
            if ( result.Succeeded )
                return new NoContent();
            else
                return BadRequest();
        }

        // DELETE: api/User/5
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (!await IsAdmin())
                return new NoAuthorized();

            return NotFound();
        }

        private async Task<bool> IsAdmin( )
        {
            return await UserManager.IsInRoleAsync( User.Identity.GetUserId(), Roles.Admin );
        }


        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
