using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using PartyService.ControllerModels;
using PartyService.DatabaseModels;
using PartyService.Models;

namespace PartyService.Providers
{
    internal class DbLocationProvider:ILocationProvider
    {
        public ApplicationUserManager UserManager { get; set; }

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
                    IsInactive = false,
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
            }

            return await GetLocationAsync( userId, id );
        }

        public async Task<ResultSet<LocationDetail>> GetLocationAsync( string userId, Guid locationId )
        {
            using (var db = new ApplicationDbContext())
            {
                Location location;
                if (await UserManager.IsInRoleAsync(userId, Roles.Admin))
                {
                    location = await db.Locations
                        .SingleOrDefaultAsync( l => !l.IsInactive && l.Id == locationId );
                }
                else
                {
                    location = await db.AdministrateLocations
                        .Where( x => x.UserId == userId )
                        .Join( db.Locations, x => x.LocationId, x => x.Id, ( a, l ) => l )
                        .SingleOrDefaultAsync( l => !l.IsInactive && l.Id == locationId );
                }

                if ( location == null )
                    return new ResultSet<LocationDetail>( false, "Location not found!" );

                return new ResultSet<LocationDetail>(true) { Result = Convert( location ) };
            }
        }

        public async Task<Result> ChangeLocationAsync( UpdateLocation u )
        {
            using ( var db = new ApplicationDbContext() )
            {
                try
                {
                    var location = await db.Locations.SingleOrDefaultAsync(x => x.Id == u.Id);

                    if (location == null)
                        return new Result(false, "Location not found!");

                    if (!string.IsNullOrEmpty(u.Address))
                        location.Street = u.Address;

                    if (!string.IsNullOrEmpty(u.AddressAdditions))
                        location.StreetAddition = u.AddressAdditions;

                    if (!string.IsNullOrEmpty(u.City))
                        location.City = u.City;

                    if (!string.IsNullOrEmpty(u.Country))
                        location.Country = u.Country;

                    if (!string.IsNullOrEmpty(u.Name))
                        location.Name = u.Name;

                    if (!string.IsNullOrEmpty(u.Website))
                        location.Website = u.Website;

                    if (!string.IsNullOrEmpty(u.ZipCode))
                        location.PostalCode = u.ZipCode;

                    if (u.Latitude.HasValue && u.Longitude.HasValue)
                        location.Position = GeographyHelper.CreatePoint(u.Latitude.Value, u.Longitude.Value);
                    else if (location.Position != null && (u.Latitude.HasValue || u.Longitude.HasValue))
                        location.Position = GeographyHelper.CreatePoint(u.Latitude ?? location.Position.Latitude.Value, u.Longitude ?? location.Position.Longitude.Value);

                    if (u.MaxAttends.HasValue)
                        location.TotalParticipants = u.MaxAttends.Value;

                    db.Locations.AddOrUpdate(location);
                    await db.SaveChangesAsync();
                    return new Result(true);
                }
                catch ( Exception exception )
                {
                    return new Result( false, exception.Message );
                }
            }
        }

        public async Task<ResultSet<LocationDetail[]>> GetLocationsAsync( string userId )
        {
            using ( var db = new ApplicationDbContext() )
            {
                LocationDetail[] locations = null;
                if ( await UserManager.IsInRoleAsync( userId, Roles.Admin ) )
                {
                    locations = (await db.Locations
                    .Where(l => !l.IsInactive)
                    .ToArrayAsync())
                    .Select(Convert)
                    .ToArray();
                }
                else
                {
                    locations = (await db.AdministrateLocations
                    .Where(x => x.UserId == userId)
                    .Join(db.Locations, x => x.LocationId, x => x.Id, (a, l) => l)
                    .Where(l => !l.IsInactive)
                    .ToArrayAsync())
                    .Select(Convert)
                    .ToArray();
                }
                return new ResultSet<LocationDetail[]>( true ) { Result = locations };
            }
        }

        public async Task RemoveLocationAsync( string userId, Guid locationId )
        {
            using ( var db = new ApplicationDbContext() )
            {
                Location location = null;
                if ( await UserManager.IsInRoleAsync( userId, Roles.Admin ) )
                {
                    location = await db.Locations.SingleAsync( x => x.Id == locationId );
                }
                else
                {
                    location = await db.AdministrateLocations.Where( x => x.UserId == userId )
                        .Join( db.Locations, x => x.LocationId, x => x.Id, ( a, l ) => l )
                        .SingleAsync( x => x.Id == locationId );
                }
                
                location.IsInactive = true;
                db.Locations.AddOrUpdate(location);
                await db.SaveChangesAsync();
            }
        }

        public async Task<bool> LocationExistAsync( string userId, Guid locationId )
        {
            using (var db = new ApplicationDbContext())
            {
                if (await UserManager.IsInRoleAsync(userId, Roles.Admin))
                {
                    return await db.Locations.AnyAsync( x => !x.IsInactive && x.Id == locationId );
                }
                else
                {
                    return await db.AdministrateLocations
                        .Where( x => x.UserId == userId )
                        .Join( db.Locations, x => x.LocationId, x => x.Id, ( a, l ) => l )
                        .AnyAsync( l => !l.IsInactive );
                }
            }
        }

        public async Task AddOwnerAsync( Guid locationId, string userId )
        {
            using ( var db = new ApplicationDbContext() )
            {
                db.AdministrateLocations.Add( new AdministrateLocation { UserId = userId, LocationId = locationId, IsInactive = false } );
                await db.SaveChangesAsync();
            }
        }

        public async Task RemoveOwnerAsync( Guid locationId, string userId )
        {
            using ( var db = new ApplicationDbContext() )
            {
                var owner = await db.AdministrateLocations.SingleOrDefaultAsync( x => x.UserId == userId && x.LocationId == locationId );
                if ( owner == null )
                    return;

                db.AdministrateLocations.Remove( owner );
                await db.SaveChangesAsync();
            }
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
                Owners =
                    location.AdministrateLocations == null
                        ? Enumerable.Empty<LocationOwner>().ToList()
                        : location.AdministrateLocations.Select( x => new LocationOwner { Id = x.UserId, LoginName = x.User.UserName } ).ToList()
            };
        }
    }
}