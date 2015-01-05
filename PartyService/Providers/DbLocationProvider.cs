using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using PartyService.ControllerModels;
using PartyService.DatabaseModels;
using PartyService.Models;

namespace PartyService.Providers
{
    public class DbLocationProvider:ILocationProvider
    {
        public async Task<ResultSet<LocationDetail>> AddLocationAsync( AddLocation addLocation, string userId )
        {
            var id = Guid.NewGuid();
            using ( var db = new ApplicationDbContext() )
            {
                db.Locations.Add( new Location
                {
                    Id = id,
                    AdministrateLocations = new[] { new AdministrateLocation { LocationId = id, UserId = userId } },
                    City = addLocation.City,
                    Name = addLocation.Name,
                    IsInactive = true,
                    PostalCode = addLocation.ZipCode,
                    Street = addLocation.Address,
                    StreetAddition = addLocation.AddressAdditions,
                    TotalParticipants = addLocation.MaxAttends,
                    Position = addLocation.Latitude.HasValue && addLocation.Longitude.HasValue
                            ? GeographyHelper.CreatePoint( addLocation.Latitude.Value, addLocation.Longitude.Value )
                            : null,
                    Country = addLocation.Country,
                    Website = addLocation.Website
                } );
                await db.SaveChangesAsync();
                var createdLocation = await db.Locations.SingleAsync( x => x.Id == id );
                return new ResultSet<LocationDetail>( true ) { Result = Convert( createdLocation ) };
            }
        }

        public async Task<Result> ChangeLocationAsync( UpdateLocation u )
        {
            using ( var db = new ApplicationDbContext() )
            {
                var location = await db.Locations.SingleOrDefaultAsync( x => x.Id == u.Id );

                if ( location == null )
                    return new Result( false, "Location not found!" );

                if ( !string.IsNullOrEmpty( u.Address ) )
                    location.Street = u.Address;

                if ( !string.IsNullOrEmpty( u.AddressAdditions ) )
                    location.StreetAddition = u.AddressAdditions;

                if ( !string.IsNullOrEmpty( u.City ) )
                    location.City = u.City;

                if ( !string.IsNullOrEmpty( u.Country ) )
                    location.Country = u.Country;
                
                if ( !string.IsNullOrEmpty( u.Name ) )
                    location.Name = u.Name;

                if ( !string.IsNullOrEmpty( u.Website ) )
                    location.Website = u.Website;

                if ( !string.IsNullOrEmpty( u.ZipCode ) )
                    location.PostalCode = u.ZipCode;

                if ( u.Latitude.HasValue && u.Longitude.HasValue )
                    location.Position = GeographyHelper.CreatePoint( u.Latitude.Value, u.Longitude.Value );
                else if ( location.Position != null && ( u.Latitude.HasValue || u.Longitude.HasValue ) )
                    location.Position = GeographyHelper.CreatePoint( u.Latitude ?? location.Position.Latitude.Value, u.Longitude ?? location.Position.Longitude.Value );

                if ( u.MaxAttends.HasValue )
                    location.TotalParticipants = u.MaxAttends.Value;
                
                db.Locations.AddOrUpdate( location );
                await db.SaveChangesAsync();
                return new Result( true );
            }
        }

        public Task<ResultSet<LocationDetail>> GetAllAsync( string userId )
        {
            throw new NotImplementedException();
        }

        public Task<Result> RemoveAsync( Guid locationId )
        {
            throw new NotImplementedException();
        }

        private LocationDetail Convert( Location location )
        {
            return new LocationDetail
            {
                Address = location.Street,
                Id = location.Id,
                Name = location.Name,
                City = location.City,
                Latitude = location.Position != null ? location.Position.Latitude : null,
                Longitude = location.Position != null ? location.Position.Longitude : null,
                MaxAttends = location.TotalParticipants,
                AddressAdditions = location.StreetAddition,
                Country = location.Country,
                ZipCode = location.PostalCode,
                Owners = location.AdministrateLocations.Select( x => new LocationOwner { Id = x.UserId, LoginName = x.User.UserName } ).ToList()
            };
        }
    }
}