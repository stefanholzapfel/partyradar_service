using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PartyService.ControllerModels;
using PartyService.Models;

namespace PartyService.Providers
{
    public interface IWebEventProvider
    {
        Task<ResultSet<List<EventDetail>>> GetEventsAsync( );
        Task<ResultSet<EventDetail>> GetEventAsync( Guid eventId );
        Task<ResultSet<List<EventDetail>>> GetEventsByLocationAsync( Guid locationId );
        Task<Result> RemoveEventAsync( Guid eventId );
        Task<bool> EventExistAsync( Guid eventId );
    }
}