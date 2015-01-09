using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PartyService.ControllerModels;

namespace PartyService.Providers
{
    public interface IAppEventProvider
    {
        Task<DateTime?> AttendEventAsync(string userId, Guid eventId);
        Task<DateTime?> LeavingEventAsync( string userId, DateTime leaveTime );
        Task<List<Event>> GetEventsAsync( double longitude, double latitude, double? radius, DateTime start, DateTime end );
        Task<Event> GetEventAsync( Guid eventId );
    }
}