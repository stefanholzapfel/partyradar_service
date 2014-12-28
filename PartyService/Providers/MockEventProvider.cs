using PartyService.ControllerModels.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace PartyService.Providers
{
    public class MockEventProvider:IEventProvider
    {
        public async System.Threading.Tasks.Task<VisitResult> AttendEventAsync(string userId, Guid eventId)
        {
            return await Task.FromResult<VisitResult>(new VisitResult { HttpCode = HttpStatusCode.Created, Timestamp = DateTime.Now });
        }

        public async System.Threading.Tasks.Task<VisitResult> LeavingEventAsync(string userId, Guid eventId)
        {
            return await Task.FromResult<VisitResult>(new VisitResult { HttpCode = HttpStatusCode.Created, Timestamp = DateTime.Now });
        }

        public async System.Threading.Tasks.Task<List<ControllerModels.App.Event>> GetEventsAsync(double longitude, double latitude, double radius)
        {
            var result = new List<Event>();
            for(int i=0; i< 10 ; i++)
            {
                result.Add(new Event
                {
                    EventId = Guid.NewGuid(),
                    End = null,
                    Start = DateTime.Now.Subtract(new TimeSpan(-2, 0, 0)),
                    LocationId = Guid.NewGuid(),
                    Title = string.Format("Party {0}", i)
                });
            }
            return await Task.FromResult<List<ControllerModels.App.Event>>(result);
        }

        public async System.Threading.Tasks.Task<ControllerModels.App.EventDetail> GetEventDetailsAsync(Guid eventId)
        {
            return await Task.FromResult<ControllerModels.App.EventDetail>(new EventDetail
            {
                EventId = eventId,
                End = null,
                Start = DateTime.Now.Subtract(new TimeSpan(-2, 0, 0)),
                LocationId = Guid.NewGuid(),
                Title = "Party-Detail",
                AttendeeCount = 300,
                City = "Wien",
                Description = "Die Party des Jahrhunderts",
                LocationName = "Die Location",
                Website = "http://www.ich-will-spass.at",
                ZipCode = "1050"
            });            
        }
    }
}