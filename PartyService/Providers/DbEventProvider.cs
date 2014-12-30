using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PartyService.ControllerModels.App;
using PartyService.DatabaseModels;
using PartyService.Models;
using WebGrease.Css.Extensions;
using Event = PartyService.DatabaseModels.Event;

namespace PartyService.Providers
{
    public class DbEventProvider:IEventProvider
    {
        public async Task<DateTime?> AttendEventAsync(string userId, Guid eventId)
        {
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

        public async Task<List<ControllerModels.App.Event>> GetEventsAsync( double longitude, double latitude, double? radius, DateTime start, DateTime end )
        {
            var curLocation = GeographyHelper.CreatePoint( latitude, longitude );
            double myRadius = radius ?? double.MaxValue;
            using ( var db = new ApplicationDbContext() )
            {
                var events = await db.Events.Where( x => x.StartTime.HasValue && x.StartTime >= start
                        && (!x.EndTime.HasValue || x.EndTime.Value <= end) && x.Location.Position.Distance(curLocation) <= myRadius)
                    .GroupJoin( db.UserOnEvents.Where( x => !x.EndTime.HasValue ), e => e.Id, a => a.Event.Id, ( e, a ) => new DbEvent { Event = e, Count = a.Count() } )
                    .OrderBy( x => x.Event.Location.Position.Distance( curLocation ) )
                    .ToArrayAsync();

                return events
                    .Select( x => Convert( x, curLocation ) )
                    .ToList();
            }
        }

        public async Task<ControllerModels.App.Event> GetEventAsync(Guid eventId)
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

        private ControllerModels.App.Event Convert( DbEvent @event, DbGeography curLocation = null )
        {
            return new ControllerModels.App.Event
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

        private class DbEvent
        {
            public Event Event { get; set; }
            public int Count { get; set; }
        }
    }
}