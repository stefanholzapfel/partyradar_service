using PartyService.ControllerModels.App;
using PartyService.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PartyService.Controllers
{
    [RoutePrefix("api/App")]
    public class AppController : ApiController
    {
        [Authorize]
        [Route("LoginEvent")]
        public async Task<IHttpActionResult> LoginEvent(Guid eventId)
        {
            return BadRequest(ModelState);
        }

        [Authorize]
        [Route("LogoutEvent")]
        public async Task<IHttpActionResult> LogoutEvent(Guid eventId)
        {
            return BadRequest(ModelState);
        }

        [Authorize]
        [Route("GetUserDetails")]
        public async Task<IHttpActionResult> GetUserDetails()
        {
            return BadRequest(ModelState);
        }

        [Route("GetEvents")]
        //[ModelName(typeof(ControllerModels.App.Event))]
        public async Task<IEnumerable<Event>> GetEventsAsync(double longitude, double latitude, double radius)
        {
            return await EventProviderFactory
                .Create(WebApiApplication.ProviderMode)
                .GetEventsAsync(longitude, latitude, radius);
        }

        [Route("GetEventDetails")]
        public async Task<EventDetail> GetEventDetails(Guid eventId)
        {
            return await EventProviderFactory
                .Create(WebApiApplication.ProviderMode)
                .GetEventDetailsAsync(eventId);
        }

        [Route("GetEventPicture")]
        public async Task<IHttpActionResult> GetEventPicture(Guid eventId)
        {
            return BadRequest(ModelState);
        }
    }
}
