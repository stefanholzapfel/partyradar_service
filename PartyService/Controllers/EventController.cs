using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using PartyService.Providers;

namespace PartyService.Controllers
{
    [Authorize]
    public class EventController : ABaseApiController
    {
        // GET: api/Event
        public async Task<IHttpActionResult> Get()
        {
            var result = await EventProviderFactory.CreateWebEventProvider( UserId, UserManager )
                .GetEventsAsync();

            if ( result.Succeeded )
                return Ok( result.Result );

            return BadRequest( result.ErrorMessage );
        }

        // GET: api/Event/5
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var provider = EventProviderFactory.CreateWebEventProvider( UserId, UserManager );
            if ( !await provider.EventExistAsync( id ) )
                return NotFound();

            var result = await provider.GetEventAsync( id );
            if ( result.Succeeded )
                return Ok( result.Result );

            return BadRequest( result.ErrorMessage );
        }

        // POST: api/Event
        public Task<IHttpActionResult> Post([FromBody]string value)
        {
        }

        // PUT: api/Event/5
        public Task<IHttpActionResult> Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Event/5
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            var provider = EventProviderFactory.CreateWebEventProvider( UserId, UserManager );
            if ( ! await provider.EventExistAsync( id ) )
                return NotFound();

            var result = await provider.RemoveEventAsync( id );
            if ( result.Succeeded )
                return new NoContent();

            return BadRequest( result.ErrorMessage );
        }
    }
}
