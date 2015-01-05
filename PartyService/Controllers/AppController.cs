using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PartyService.ControllerModels.App;
using PartyService.Providers;

namespace PartyService.Controllers
{
    [RoutePrefix("api/App")]
    public class AppController : ApiController
    {
        [Authorize]
        [Route("LoginEvent")]
        public async Task<IHttpActionResult> LoginEvent(Guid eventId)
        {
            if ( eventId == Guid.Empty )
                return await Task.FromResult( BadRequest(string.Format( "{0} is not a valid id!",Guid.Empty )) );

            var result = await EventProviderFactory
                .Create( WebApiApplication.ProviderMode )
                .AttendEventAsync( User.Identity.GetUserId(), eventId );

            if ( result.HasValue )
                return await Task.FromResult( Ok( result.Value ) );
            
            return await Task.FromResult( NotFound() );
        }

        [Authorize]
        [Route("LogoutEvent")]
        public async Task<IHttpActionResult> LogoutEvent(Guid eventId)
        {
            var now = DateTime.Now;
            var result = await EventProviderFactory
                .Create( WebApiApplication.ProviderMode )
                .LeavingEventAsync( User.Identity.GetUserId(), now );

            if(result.HasValue)
               return await Task.FromResult( Ok( result.Value ) );

            return await Task.FromResult( NotFound() );
        }

        [Authorize]
        [Route("GetUserDetails")]
        public async Task<IHttpActionResult> GetUserDetails()
        {
            var details = await UserProviderFactory
                .Create()
                .GetAppUserDetailAsync( this.User.Identity.GetUserId() );

            return Ok( details );
        }

        [Route("GetEvents")]
        public async Task<IEnumerable<Event>> GetEventsAsync(double longitude, double latitude, DateTime start, DateTime end, double? radius= null)
        {
            var result = await EventProviderFactory
                .Create(WebApiApplication.ProviderMode)
                .GetEventsAsync(longitude, latitude, radius, start, end );

            result.ForEach( x => x.CreateUrls( Url ) );
            return await Task.FromResult<IEnumerable<Event>>(result);            
        }

        [Route("GetEvent")]
        public async Task<Event> GetEvent(Guid eventId)
        {
            var result = await EventProviderFactory
                .Create(WebApiApplication.ProviderMode)
                .GetEventAsync(eventId);

            if ( result != null )
                result.CreateUrls( Url );

            return await Task.FromResult<Event>(result);
        }

        [Route("GetEventPicture")]
        public async Task<IHttpActionResult> GetEventPicture(Guid eventId)
        {
            if (eventId == Guid.Empty)
                return BadRequest();

            var pic = await PictureProviderFactory
                .Create(WebApiApplication.ProviderMode)
                .GetEventPictureAsync(eventId);

            if (pic == null)
                return NotFound();

            return Ok(pic);
        }
    }
}
