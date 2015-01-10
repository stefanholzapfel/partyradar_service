using System;
using System.Threading.Tasks;
using System.Web.Http;
using PartyService.ControllerModels;
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
            {
                result.Result.ForEach( x=>x.CreateUrls( Url ) );
                return Ok( result.Result );
            }

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
            {
                result.Result.CreateUrls( Url );
                return Ok( result.Result );
            }

            return BadRequest( result.ErrorMessage );
        }

        // POST: api/Event
        public async Task<IHttpActionResult> Post([FromBody]AddEvent model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (!await LocationProviderFactory.Create( UserManager ).LocationExistAsync( UserId, model.LocationId))
                return NotFound();

            var result = await EventProviderFactory.CreateWebEventProvider( UserId, UserManager ).AddEventAsync( model );

            if ( result.Succeeded )
                return Ok( result.Result );

            return BadRequest( result.ErrorMessage );
        }

        // PUT: api/Event/5
        public async Task<IHttpActionResult> Put(Guid id, ControllerChangeEvent changeEvent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (changeEvent.LocationId.HasValue && !await LocationProviderFactory.Create(UserManager).LocationExistAsync(UserId, changeEvent.LocationId.Value))
                return NotFound();

            var provider = EventProviderFactory.CreateWebEventProvider( UserId, UserManager );
            if (!await provider.EventExistAsync(id))
                return NotFound();

            var result = await provider.ChangeEventAsync( Convert( changeEvent, id ) );

            if ( result.Succeeded )
                return new NoContent();

            return BadRequest( result.ErrorMessage );
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

        private ChangeEvent Convert( ControllerChangeEvent changeEvent, Guid id )
        {
            return new ChangeEvent
            {
                EventId = id,
                Description = changeEvent.Description,
                Website = changeEvent.Website,
                End = changeEvent.End,
                Image = changeEvent.Image,
                LocationId = changeEvent.LocationId,
                MaxAttends = changeEvent.MaxAttends,
                Start = changeEvent.Start,
                Title = changeEvent.Title,
                KeywordIds = changeEvent.KeywordIds
            };
        }
    }
}
