using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PartyService.Providers
{
    public class DbEventProvider:IEventProvider
    {
        public async System.Threading.Tasks.Task<VisitResult> AttendEventAsync(string userId, Guid eventId)
        {
            throw new NotImplementedException();
        }

        public async System.Threading.Tasks.Task<VisitResult> LeavingEventAsync(string userId, Guid eventId)
        {
            throw new NotImplementedException();
        }

        public async System.Threading.Tasks.Task<List<ControllerModels.App.Event>> GetEventsAsync(double longitude, double latitude, double radius)
        {
            throw new NotImplementedException();
        }

        public async System.Threading.Tasks.Task<ControllerModels.App.EventDetail> GetEventDetailsAsync(Guid eventId)
        {
            throw new NotImplementedException();
        }
    }
}