using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using PartyService.ControllerModels;
using PartyService.DatabaseModels;
using PartyService.Providers;

namespace PartyService.Controllers
{
    [Authorize]
    public class OldLocationController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Location
        public IEnumerable<LocationDetail> GetLocations()
        {
            return db.AdministrateLocations
                .Where( x => x.UserId == this.User.Identity.GetUserId() )
                .Join( db.Locations, x => x.LocationId, x => x.Id, ( a, l ) => l )
                .Where( l => ! l.IsInactive )
                .Select( LocationDetail.Convert );
        }

        // GET: api/Location/5
        [ResponseType(typeof(LocationDetail))]
        public async Task<IHttpActionResult> GetLocation(Guid id)
        {
            var location = await db.AdministrateLocations
                .SingleOrDefaultAsync(x => x.UserId == this.User.Identity.GetUserId() && x.LocationId == id );
                
            if (location == null)
            {
                return NotFound();
            }

            return Ok(LocationDetail.Convert( location.Location ));
        }

        // PUT: api/Location/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLocation(Guid id, AddLocation model)
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

        private DatabaseModels.Location Convert(Guid id, AddLocation model )
        {
            DbGeography position = null;
            if ( model.Latitude.HasValue && model.Longitude.HasValue )
                position = GeographyHelper.CreatePoint( model.Latitude.Value, model.Longitude.Value );

            return new DatabaseModels.Location
            {
                Id =id,
                Name = model.Name,
                City = model.City,
                PostalCode = model.ZipCode,
                TotalParticipants = model.MaxAttends,
                Street = model.Address,
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

        private void FillLocation(DatabaseModels.Location location, UpdateLocation model )
        {
            DbGeography position = null;
            if (model.Latitude.HasValue && model.Longitude.HasValue)
                position = GeographyHelper.CreatePoint(model.Latitude.Value, model.Longitude.Value);

            location.Name = model.Name;
            location.City = model.City;
            location.PostalCode = model.ZipCode;
            location.Street = model.Address;
            location.TotalParticipants = model.MaxAttends;
        }

        // POST: api/Location
        [ResponseType(typeof(LocationDetail))]
        public async Task<IHttpActionResult> PostLocation(UpdateLocation model)
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
        [ResponseType(typeof(LocationDetail))]
        public async Task<IHttpActionResult> DeleteLocation(Guid id)
        {
            DatabaseModels.Location location = await db.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            location.IsInactive = true;
            db.Locations.AddOrUpdate( location);
            await db.SaveChangesAsync();

            return Ok( LocationDetail.Convert( location ) );
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