using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using PartyService.Controllers;
using PartyService.Providers;

namespace PartyService.ControllerModels
{
    public class LocationOwnerController : ABaseApiController
    {
        // GET: api/LocationOwner
        public IHttpActionResult Get()
        {
            return BadRequest("change owner is not implemented!");
        }

        // GET: api/LocationOwner/5
        public IHttpActionResult Get(int id)
        {
            return BadRequest("change owner is not implemented!");
        }

        // POST: api/LocationOwner
        public async Task<IHttpActionResult> Post(Guid id, string ownerId)
        {
            var provider = LocationProviderFactory.Create(UserManager);
            var aUserFound = UserProviderFactory.Create().UserExistAsync(ownerId);
            var aLocationFound = provider.LocationExistAsync(User.Identity.GetUserId(), id);

            if (!(await aLocationFound) || !(await aUserFound))
                return NotFound();

            await provider.AddOwnerAsync(id, ownerId);
            return new NoContent();
        }

        // PUT: api/LocationOwner/5
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            return BadRequest("change owner is not implemented!");
        }
        
        public async Task<IHttpActionResult> Delete(Guid id, string ownerId)
        {
            var provider = LocationProviderFactory.Create(UserManager);
            var aUserFound = UserProviderFactory.Create().UserExistAsync(ownerId);
            var aLocationFound = provider.LocationExistAsync(User.Identity.GetUserId(), id);

            if (!(await aLocationFound) || !(await aUserFound))
                return NotFound();

            await provider.RemoveOwnerAsync(id, ownerId);
            return new NoContent();
        }
    }
}
