﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using PartyService.ControllerModels;
using PartyService.Providers;

namespace PartyService.Controllers
{
    [Authorize]
    public class LocationController : ABaseApiController
    {
        // GET: api/Location
        public async Task<IHttpActionResult> Get()
        {
            var result = await LocationProviderFactory.Create(UserManager).GetLocationsAsync( User.Identity.GetUserId() );
            
            if(result.Succeeded)
                return Ok( result );

            return BadRequest( result.ErrorMessage );
        }

        // GET: api/Location/5
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var provider = LocationProviderFactory.Create(UserManager);
            var userId = User.Identity.GetUserId();

            if ( await provider.LocationExistAsync( userId, id ) )
            {
                var result = await provider.GetLocationAsync( userId, id );

                if ( result.Succeeded )
                {
                    var location = result.Result;
                    return Ok( location );
                }
                else
                    return BadRequest( result.ErrorMessage );
            }
            else
                return NotFound();
        }

        // POST: api/Location
        public async Task<IHttpActionResult> Post([FromBody]AddLocation addLocation)
        {
            if ( !ModelState.IsValid )
                return BadRequest( ModelState );

            var result = await LocationProviderFactory.Create(UserManager).AddLocationAsync( addLocation, User.Identity.GetUserId() );

            if ( result.Succeeded )
                return Ok( result.Result );
            else
                return BadRequest( result.ErrorMessage );
        }

        // PUT: api/Location/5
        public async Task<IHttpActionResult> Put(Guid id, [FromBody]UpdateLocation updateLocation)
        {
            var userId = User.Identity.GetUserId();
            var provider = LocationProviderFactory.Create( UserManager );

            var locationFound = await provider.LocationExistAsync( userId, id );
            if (! locationFound )
                return NotFound();

            updateLocation.Id = id;
            var result = await provider.ChangeLocationAsync( updateLocation );

            if ( result.Succeeded )
                return new NoContent();

            return BadRequest( result.ErrorMessage );
        }

        // DELETE: api/Location/5
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            var userId = User.Identity.GetUserId();
            var provider = LocationProviderFactory.Create(UserManager);

            var locationFound = await provider.LocationExistAsync(userId, id);
            if (!locationFound)
                return NotFound();

            await provider.RemoveLocationAsync( userId, id );
            return new NoContent();
        }
    }
}
