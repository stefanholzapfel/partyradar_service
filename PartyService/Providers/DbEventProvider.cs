using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
using PartyService.ControllerModels;
using PartyService.DatabaseModels;
using PartyService.Models;
using Event = PartyService.DatabaseModels.Event;

namespace PartyService.Providers
{
    public class DbEventProvider:IAppEventProvider
    {
        public async Task<DateTime?> AttendEventAsync(string userId, Guid eventId)
        {
            if ( !await EventExistAsync( eventId ) )
                return null;

            var now = DateTime.Now;
            using ( var db = new ApplicationDbContext() )
            {
                if ( db.UserOnEvents.Any( x => !x.EndTime.HasValue && x.User.Id == userId ) )
                    await LeavingEventAsync( userId, now );

                db.UserOnEvents.Add( new UserOnEvent
                {
                    BeginTime = now,
                    EndTime = null,
                    EventId = eventId,
                    Id = Guid.NewGuid(),
                    IsInactive = false,
                    UserId = userId
                } );

                db.SaveChanges();
            }
            return now;
        }

        public async Task<DateTime?> LeavingEventAsync( string userId, DateTime leaveTime )
        {
            using ( var db = new ApplicationDbContext() )
            {
                var currentAttendParties = db.UserOnEvents.Where( x => !x.EndTime.HasValue && x.User.Id == userId );
                await currentAttendParties.ForEachAsync( x => x.EndTime = leaveTime );
                await db.SaveChangesAsync();
                return leaveTime;
            }
        }

        public async Task<List<ControllerModels.Event>> GetEventsAsync( double longitude, double latitude, double? radius, DateTime start, DateTime end )
        {
            var curLocation = GeographyHelper.CreatePoint( latitude, longitude );
            double myRadius = radius ?? double.MaxValue;
            using ( var db = new ApplicationDbContext() )
            {
                var events = await GetActiveEvents( db )
                    .Where( x => x.StartTime.HasValue && x.StartTime >= start
                        && (!x.EndTime.HasValue || x.EndTime.Value <= end) && x.Location.Position.Distance(curLocation) <= myRadius)
                    .GroupJoin( db.UserOnEvents.Where( x => !x.EndTime.HasValue ), e => e.Id, a => a.Event.Id, ( e, a ) => new DbEvent { Event = e, Count = a.Count() } )
                    .OrderBy( x => x.Event.Location.Position.Distance( curLocation ) )
                    .ToArrayAsync();

                return events
                    .Select( x => Convert( x, curLocation ) )
                    .ToList();
            }
        }

        public async Task<ControllerModels.Event> GetEventAsync(Guid eventId)
        {
            using ( var db = new ApplicationDbContext() )
            {
                var groupJoin = await db.Events
                    .Where( x => x.Id == eventId )
                    .GroupJoin( db.UserOnEvents.Where( x => !x.EndTime.HasValue ), e => e.Id, a => a.Event.Id, ( e, a ) => new DbEvent{ Event = e, Count = a.Count() } )
                    .ToArrayAsync();

                if ( groupJoin.Count() != 1 )
                    return null;

                return Convert( groupJoin.Single() );
            }
        }

        protected ControllerModels.Event Convert( DbEvent @event, DbGeography curLocation = null )
        {
            return new ControllerModels.Event
            {
                EventId = @event.Event.Id,
                Start = @event.Event.StartTime,
                End = @event.Event.EndTime,
                LocationId = @event.Event.Location.Id,
                LocationName = @event.Event.Location.Name,
                ZipCode = @event.Event.Location.PostalCode,
                City = @event.Event.Location.City,
                Keywords =
                    @event.Event.EventKeywords == null
                        ? Enumerable.Empty<ControllerModels.Keyword>().ToList()
                        : @event.Event.EventKeywords.Select( k => new ControllerModels.Keyword { Id = k.KeywordId, Label = k.Keyword.Label } ).ToList(),
                Latitude = @event.Event.Location.Position.Latitude ?? 0.0,
                Longitude = @event.Event.Location.Position.Longitude ?? 0.0,
                Title = @event.Event.Name,
                AttendeeCount = @event.Count,
                Distance = curLocation == null ? null : @event.Event.Location.Position.Distance( curLocation ),
                Adress = @event.Event.Location.Street,
                AdressAdditions = @event.Event.Location.StreetAddition,
                Country = @event.Event.Location.Country,
                Description = @event.Event.Description,
                MaxAttends = @event.Event.TotalParticipants?? @event.Event.Location.TotalParticipants,
                Website = @event.Event.Website ?? @event.Event.Location.Website
            };
        }

        protected IQueryable<DatabaseModels.Event> GetActiveEvents( ApplicationDbContext db )
        {
            return db.Events.Where( x => !x.IsInactive );
        }
        private async Task<bool> EventExistAsync(Guid eventId)
        {
            using ( var db = new ApplicationDbContext() )
            {
                return await db.Events.AnyAsync( x => !x.IsInactive && x.Id == eventId );
            }
        }

        protected class DbEvent
        {
            public Event Event { get; set; }
            public int Count { get; set; }
        }
    }

    public class DbWebEventProvider: DbEventProvider, IWebEventProvider
    {
        public ApplicationUserManager UserManager { get; set; }
        public string UserId { get; set; }

        public async Task<ResultSet<List<EventDetail>>> GetEventsAsync( )
        {
            using (var db = new ApplicationDbContext())
            {
                IQueryable<Event> eventQuery = await GetEventsDependingOnOwner(db);
                
                var events = await eventQuery
                    .GroupJoin(db.UserOnEvents.Where(x => !x.EndTime.HasValue), e => e.Id, a => a.Event.Id, (e, a) => new DbEvent { Event = e, Count = a.Count() })
                    .OrderBy(x => x.Event.Name)
                    .ToArrayAsync();

                return new ResultSet<List<EventDetail>>( true )
                {
                    Result = events.Select( x => ConvertDetail( x ) ).ToList()
                };
            }
        }

        public new async Task<ResultSet<EventDetail>> GetEventAsync( Guid eventId )
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    IQueryable<Event> eventQuery = await GetEventsDependingOnOwner(db);

                    var @event = await eventQuery
                        .GroupJoin(db.UserOnEvents.Where(x => !x.EndTime.HasValue), e => e.Id, a => a.Event.Id, (e, a) => new DbEvent { Event = e, Count = a.Count() })
                        .SingleOrDefaultAsync(x => x.Event.Id == eventId);

                    if (@event == null)
                        return new ResultSet<EventDetail>(false, "No event found for given ID!");

                    return new ResultSet<EventDetail>(true)
                    {
                        Result = ConvertDetail(@event)
                    };
                }
                catch ( Exception exception)
                {
                    return new ResultSet<EventDetail>( false, exception.Message );
                }
            }
        }

        public async Task<ResultSet<List<EventDetail>>> GetEventsByLocationAsync( Guid locationId )
        {
            using ( var db = new ApplicationDbContext() )
            {
                var eventQuery = await GetEventsDependingOnOwner( db );

                try
                {
                    var events = await eventQuery
                    .Where(x => x.Location.Id == locationId)
                   .GroupJoin(db.UserOnEvents.Where(x => !x.EndTime.HasValue), e => e.Id, a => a.Event.Id, (e, a) => new DbEvent { Event = e, Count = a.Count() })
                   .OrderBy(x => x.Event.Name)
                   .ToArrayAsync();

                    return new ResultSet<List<EventDetail>>(true)
                    {
                        Result = events.Select(x => ConvertDetail(x)).ToList()
                    };
                }
                catch ( Exception exc )
                {
                    return new ResultSet<List<EventDetail>>( false, exc.Message );
                }
            }
        }

        public async Task<Result> RemoveEventAsync( Guid eventId )
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var eventQuery = await GetEventsDependingOnOwner(db);
                    var @event = await eventQuery.Where(x => x.Id == eventId).SingleOrDefaultAsync();
                    if (@event == null)
                        return new Result(false, "No event found for given ID!");

                    db.Events.AddOrUpdate(@event);
                    await db.SaveChangesAsync();
                    return new Result(true);
                }
                catch (Exception exception)
                {
                    return new Result(false, exception.Message);
                }
            }
        }

        public async Task<bool> EventExistAsync( Guid eventId )
        {
            using ( var db = new ApplicationDbContext() )
            {
                var eventQuery = await GetEventsDependingOnOwner( db );
                return await eventQuery.AnyAsync( x => x.Id == eventId );
            }
        }
        protected EventDetail ConvertDetail(DbEvent @event, DbGeography curLocation = null)
        {
            var detailEvent = EventDetail.Create(Convert(@event, curLocation));

            if (@event.Event.Location.AdministrateLocations != null)
                detailEvent.Owners = @event.Event.Location.AdministrateLocations.Select(x => new LocationOwner { Id = x.UserId, LoginName = x.User.UserName }).ToList();

            return detailEvent;
        }

        protected async Task<IQueryable<Event>> GetEventsDependingOnOwner( ApplicationDbContext db )
        {
            IQueryable<Event> eventQuery;
            if (await UserManager.IsInRoleAsync(UserId, Roles.Admin))
                eventQuery = GetActiveEvents(db);
            else
                eventQuery = GetActiveEvents(db)
                    .Where(e => e.Location.AdministrateLocations.Any(al => al.UserId == UserId));
            
            return eventQuery;
        }
    }
}