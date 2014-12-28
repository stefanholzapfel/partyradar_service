using PartyService.ControllerModels.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PartyService.Providers
{
    public interface IEventProvider
    {
        Task<VisitResult> AttendEventAsync(string userId, Guid eventId);
        Task<VisitResult> LeavingEventAsync(string userId, Guid eventId);
        Task<List<Event>> GetEventsAsync(double longitude, double latitude, double radius);
        Task<EventDetail> GetEventDetailsAsync(Guid eventId);
    }
}