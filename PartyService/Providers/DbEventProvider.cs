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
            var result = new ControllerModels.Event
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
               
                Title = @event.Event.Name,
                AttendeeCount = @event.Count,
                Adress = @event.Event.Location.Street,
                AdressAdditions = @event.Event.Location.StreetAddition,
                Country = @event.Event.Location.Country,
                Description = @event.Event.Description,
                MaxAttends = @event.Event.TotalParticipants?? @event.Event.Location.TotalParticipants,
                Website = @event.Event.Website ?? @event.Event.Location.Website,
                HasImage = @event.Event.Image != null && @event.Event.Image.Any()
            };

            if ( @event.Event.Location.Position != null )
            {
                result.Latitude = @event.Event.Location.Position.Latitude ?? 0.0;
                result.Longitude = @event.Event.Location.Position.Longitude ?? 0.0;
                result.Distance = curLocation == null ? null : @event.Event.Location.Position.Distance( curLocation );
            }
            return result;
        }

        protected IQueryable<DatabaseModels.Event> GetActiveEvents( ApplicationDbContext db )
        {
            return db.Events.Where( x => !x.IsInactive );
        }
        public async Task<bool> EventExistAsync(Guid eventId)
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
                    return new ResultSet<EventDetail>( false, exception.JoinMessages() );
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
                    return new ResultSet<List<EventDetail>>( false, exc.JoinMessages() );
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
                    
                    @event.IsInactive = true;
                    db.Events.AddOrUpdate(@event);
                    await db.SaveChangesAsync();
                    
                    return new Result(true);
                }
                catch (Exception exception)
                {
                    return new Result(false, exception.JoinMessages());
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

        public async Task<ResultSet<EventDetail>> AddEventAsync( AddEvent addEvent )
        {
            var id = Guid.NewGuid();
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var nEvent = new Event
                    {
                        Id = id,
                        Description = addEvent.Description,
                        EndTime = addEvent.End,
                        Image = addEvent.Image != null ? System.Convert.FromBase64String(addEvent.Image): null,
                        Location = db.Locations.Single( x => x.Id == addEvent.LocationId ),
                        IsInactive = false,
                        Name = addEvent.Title,
                        Website = addEvent.Website,
                        StartTime = addEvent.Start,
                        TotalParticipants = addEvent.MaxAttends
                    };
                    
                    if ( addEvent.LocationId != Guid.Empty )
                        nEvent.Location = db.Locations.Single( x => x.Id == addEvent.LocationId );
                    
                    if (addEvent.KeywordIds != null && addEvent.KeywordIds.Any() )
                        nEvent.EventKeywords = KeywordProvider.GetKeywords( db )
                            .Where( x => addEvent.KeywordIds.Contains( x.Id ) )
                            .Select( x => new EventKeyword { EventId = id, KeywordId = x.Id } )
                            .ToList();

                    db.Events.Add(nEvent);
                    await db.SaveChangesAsync();
                }
                return await GetEventAsync( id );
            }
            catch (Exception exception)
            {
                return new ResultSet<EventDetail>(false, exception.JoinMessages());
            }
        }

        public async Task<Result> ChangeEventAsync( ChangeEvent changeEvent )
        {
            try
            {
                using ( var db = new ApplicationDbContext() )
                {
                    var @event = await ( await GetEventsDependingOnOwner( db ) ).SingleOrDefaultAsync( x => x.Id == changeEvent.EventId );

                    if ( changeEvent.Description != null )
                        @event.Description = changeEvent.Description == String.Empty ? null : changeEvent.Description;

                    if ( changeEvent.Image != null )
                        @event.Image = changeEvent.Image.Count() == 0 ? null : changeEvent.Image;

                    if ( changeEvent.End.HasValue )
                        @event.EndTime = changeEvent.End;

                    if ( changeEvent.Start.HasValue )
                        @event.StartTime = changeEvent.Start;

                    if ( changeEvent.LocationId != null )
                        @event.Location = await db.Locations.SingleAsync( x => x.Id == changeEvent.LocationId );

                    if ( changeEvent.MaxAttends.HasValue )
                        @event.TotalParticipants = changeEvent.MaxAttends;

                    if ( changeEvent.Title != null )
                        @event.Name = changeEvent.Title == String.Empty ? null : changeEvent.Title;

                    if ( changeEvent.Website != null )
                        @event.Website = changeEvent.Website == String.Empty ? null : changeEvent.Website;

                    if ( changeEvent.KeywordIds != null )
                    {
                        @event.EventKeywords.RemoveAll( x=> 1==1 );

                        @event.EventKeywords = KeywordProvider.GetKeywords( db )
                            .Where( x => changeEvent.KeywordIds.Contains( x.Id ) )
                            .Select( x => new EventKeyword { EventId = @event.Id, KeywordId = x.Id } )
                            .ToList();
                    }

                    db.Events.AddOrUpdate( @event );
                    db.SaveChanges();
                }

                return new Result( true );

            }
            catch ( Exception exception)
            {
                return new Result( false, exception.JoinMessages() );
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