﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
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
                .Create( WebApiApplication.ProviderMode )
                .GetUserDetailAsync( this.User.Identity.GetUserId() );

            return Ok( details );
        }

        [Route("GetEvents")]
        public async Task<IEnumerable<Event>> GetEventsAsync(double longitude, double latitude, DateTime start, DateTime end, double? radius= null)
        {
            var result = await EventProviderFactory
                .Create(WebApiApplication.ProviderMode)
                .GetEventsAsync(longitude, latitude, radius, start, end );

            result.ForEach(x => x.DetailUrl = Url.Link("ControllerActionApi", new
                {
                    controller = "app",
                    action = "GetEvent",
                    eventId = x.EventId.ToString()
                })
            );
            return await Task.FromResult<IEnumerable<Event>>(result);            
        }

        [Route("GetEvent")]
        public async Task<Event> GetEvent(Guid eventId)
        {
            var result = await EventProviderFactory
                .Create(WebApiApplication.ProviderMode)
                .GetEventAsync(eventId);
            
            if(result != null)
                result.DetailUrl = Url.Link("ControllerActionApi", new { controller = "app", action = "GetEvent", eventId = result.EventId.ToString() });

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
