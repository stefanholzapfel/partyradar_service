﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PartyService.ControllerModels;

namespace PartyService.Providers
{
    public class MockEventProvider:IEventProvider
    {
        public async System.Threading.Tasks.Task<DateTime?> AttendEventAsync(string userId, Guid eventId)
        {
            return await Task.FromResult(DateTime.Now);
        }

        public async Task<DateTime?> LeavingEventAsync( string userId, DateTime leaveTime )
        {
            return await Task.FromResult(DateTime.Now);
        }

        public async Task<List<Event>> GetEventsAsync( double longitude, double latitude, double? radius, DateTime start, DateTime end )
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
            return await Task.FromResult<List<Event>>(result);
        }

        public async System.Threading.Tasks.Task<Event> GetEventAsync(Guid eventId)
        {
            return await Task.FromResult<Event>(new Event
            {
                EventId = eventId,
                End = null,
                Start = DateTime.Now.Subtract(new TimeSpan(-2, 0, 0)),
                LocationId = Guid.NewGuid(),
                Title = "Party-Detail",
                AttendeeCount = 300,
                City = "Wien",
                Description = "Die Party des Jahrhunderts",
                LocationName = "Die LocationDetails",
                Website = "http://www.ich-will-spass.at",
                ZipCode = "1050"
            });            
        }
    }
}