using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using PartyService.ControllerModels;
using PartyService.DatabaseModels;
using PartyService.Models;
using WebGrease.Css.Extensions;

namespace PartyService.Controllers
{
	[Authorize]
    [RoutePrefix("api/Event")]
    public class EventController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Event
        public IEnumerable<ControllerEvent> GetEvents()
        {
	        return db.AdministrateLocations
		        .Where( x => x.UserId == this.User.Identity.GetUserId() )
		        .Join( db.Locations, x => x.LocationId, x => x.Id, ( a, l ) => l )
		        .Join( db.Events, l => l.Id, e => e.Location.Id, ( l, e ) => e )
		        .Where( e => !e.IsInactive )
				.Select( ControllerEvent.Convert );
        }

        // GET: api/Event/5
        [ResponseType(typeof(ControllerEvent))]
        public async Task<IHttpActionResult> GetEvent(Guid id)
        {
	        Event @event = await db.Events.SingleOrDefaultAsync( x => x.Id == id );
            if (@event == null || @event.IsInactive)
            {
                return NotFound();
            }

			return Ok(ControllerEvent.Convert(@event));
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
        [ResponseType(typeof(ControllerEvent))]
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

			return CreatedAtRoute("DefaultApi", new { id = model.Id }, ControllerEvent.Convert(model));
        }

        // DELETE: api/Event/5
        [ResponseType(typeof(ControllerEvent))]
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

            return Ok(ControllerEvent.Convert( model ));
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
    }
}