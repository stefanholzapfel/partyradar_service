using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using PartyService.ControllerModels;
using PartyService.Models;

namespace PartyService.Controllers
{
    [Authorize]
    public class LocationController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Location
        public IEnumerable<ControllerLocation> GetLocations()
        {
            return db.AdministrateLocations
                .Where( x => x.UserId == this.User.Identity.GetUserId() )
                .Join( db.Locations, x => x.LocationId, x => x.Id, ( a, l ) => l )
                .Where( l => ! l.IsInactive )
                .Select( ControllerLocation.Convert );
        }

        // GET: api/Location/5
        [ResponseType(typeof(ControllerLocation))]
        public async Task<IHttpActionResult> GetLocation(Guid id)
        {
            var location = await db.AdministrateLocations
                .SingleOrDefaultAsync(x => x.UserId == this.User.Identity.GetUserId() && x.LocationId == id );
                
            if (location == null)
            {
                return NotFound();
            }

            return Ok(ControllerLocation.Convert( location.Location ));
        }

        // PUT: api/Location/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLocation(Guid id, CreateLocationBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.Locations.Add( Convert(id, model ) );

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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

        private Location Convert(Guid id, CreateLocationBindingModel model )
        {
            DbGeography position = null;
            if (model.Latitude.HasValue && model.Longitude.HasValue)
                position = DbGeography.PointFromText(string.Format("POINT({0},{1})", model.Longitude, model.Latitude), 4326);

            return new Location
            {
                Id =id,
                Name = model.Name,
                Place = model.Place,
                PostalCode = model.PostalCode,
                TotalParticipants = model.TotalParticipants,
                Street = model.Street,
                AdministrateLocations = new List<AdministrateLocation>
                {
                    new AdministrateLocation
                    {
                        LocationId = id,
                        UserId = this.User.Identity.GetUserId()
                    }
                },
                Position = position
            };
        }

        private void FillLocation(Location location, UpdateLocationBindingModel model )
        {
            DbGeography position = null;
            if (model.Latitude.HasValue && model.Longitude.HasValue)
                position = DbGeography.PointFromText(string.Format("POINT({0},{1})", model.Longitude, model.Latitude), 4326);

            location.Name = model.Name;
            location.Place = model.Place;
            location.PostalCode = model.PostalCode;
            location.Street = model.Street;
            location.TotalParticipants = model.TotalParticipants;
        }

        // POST: api/Location
        [ResponseType(typeof(ControllerLocation))]
        public async Task<IHttpActionResult> PostLocation(UpdateLocationBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var location = await db.Locations.FindAsync( model.Id );
            if ( location == null )
                return StatusCode(HttpStatusCode.NotFound);
            
            FillLocation( location,model );

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LocationExists(model.Id))
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

        // DELETE: api/Location/5
        [ResponseType(typeof(ControllerLocation))]
        public async Task<IHttpActionResult> DeleteLocation(Guid id)
        {
            Location location = await db.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            location.IsInactive = true;
            db.Locations.AddOrUpdate( location);
            await db.SaveChangesAsync();

            return Ok( ControllerLocation.Convert( location ) );
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationExists(Guid id)
        {
            return db.Locations.Count(e => e.Id == id) > 0;
        }
    }
}