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
using PartyService.Models;

namespace PartyService.Controllers
{
	[Authorize]
    public class EventController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Event
        public IQueryable<Event> GetEvents()
        {
            return db.Events;
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
        public async Task<IHttpActionResult> PutEvent(Guid id, Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @event.Id)
            {
                return BadRequest();
            }

            db.Entry(@event).State = EntityState.Modified;

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
        public async Task<IHttpActionResult> PostEvent(Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Events.Add(@event);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EventExists(@event.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = @event.Id }, @event);
        }

        // DELETE: api/Event/5
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> DeleteEvent(Guid id)
        {
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

	        @event.IsInactive = true;
            db.Events.AddOrUpdate( @event );
            await db.SaveChangesAsync();

            return Ok(@event);
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
            return db.Events.Count(e => e.Id == id) > 0;
        }

		//[Route("AttendEvent")]
		//public async Task<HttpResponseMessage> AttendEvent( AttendEventBindingModel model )
		//{
		//	return 
		//}
    }
}