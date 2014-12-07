using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PartyService.Models;
using WebGrease.Css.Extensions;

namespace PartyService.Controllers
{
	[Authorize]
    public class EventController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Event
        public IQueryable<Event> GetEvents()
        {
	        db.AdministrateLocations
		        .Where( x => x.UserId == this.User.Identity.GetUserId() )
		        .Join( db.Locations, x => x.LocationId, x => x.Id, ( a, l ) => l )
		        .Join( db.Events, l => l.Id, e => e.Location.Id, ( l, e ) => e )
		        .Where( e => !e.IsInactive );
        }

        // GET: api/Event/5
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> GetEvent(Guid id)
        {
	        Event @event = await db.Events.SingleOrDefaultAsync( x => x.Id == id );
            if (@event == null || @event.IsInactive)
            {
                return NotFound();
            }

            return Ok(@event);
        }

        // PUT: api/Event/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEvent(Guid id, Event model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return BadRequest();
            }

            db.Entry(model).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Event
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> PostEvent(Event model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Events.Add(model);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EventExists(model.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = model.Id }, model);
        }

        // DELETE: api/Event/5
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> DeleteEvent(Guid id)
        {
            Event model = await db.Events.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

	        model.IsInactive = true;
            db.Events.AddOrUpdate( model );
            await db.SaveChangesAsync();

            return Ok(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(Guid id)
        {
            return db.Events.Any(e => e.Id == id);
        }

		// PUT: api/Event/Attend
		[ResponseType(typeof(void))]
		[Route("Attend")]
		public async Task<IHttpActionResult> Attend( AttendEventBindingModel model )
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != aEvent.Id)
			{
				return BadRequest();
			}
			
			var aEvent = db.Events.SingleOrDefaultAsync(x=>x.Id == model.EventId)
			var aUser = db.GetUserAsync( User.Identity.GetUserId() );
			
			var actualOpenEvents = db.UserOnEvents
				.Where( x => x.User.Id == userId && x.EndTime == null )
				.ToArray();

			if ( actualOpenEvents.Any( x => x.Event.Id == model.EventId ) )
				return StatusCode( HttpStatusCode.OK );

			var now = DateTime.Now;
			actualOpenEvents.ForEach( x=>x.EndTime = now );
			db.UserOnEvents.AddOrUpdate( actualOpenEvents );
			
			var @event = await aEvent;
			if ( @event == null )
				return BadRequest();

			var userOnEvent = new UserOnEvent { BeginTime = now, Event = @event, Id = Guid.NewGuid(), User = aUser }
			db.UserOnEvents.Add( userOnEvent );
			await db.SaveChangesAsync();
			return StatusCode(HttpStatusCode.NoContent);
		}
    }
}