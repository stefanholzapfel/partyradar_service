using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Routing;

namespace PartyService.ControllerModels
{
    public class EventDetail : Event
    {
        public ICollection<LocationOwner> Owners { get; set; }

        public static EventDetail Create( Event e )
        {
            return new EventDetail
            {
                Adress = e.Adress,
                AdressAdditions = e.AdressAdditions,
                AttendeeCount = e.AttendeeCount,
                City = e.City,
                Country = e.Country,
                Description = e.Description,
                DetailUrl = e.DetailUrl,
                Distance = e.Distance,
                End = e.End,
                EventId = e.EventId,
                ImageUrl = e.ImageUrl,
                Keywords = e.Keywords,
                Latitude = e.Latitude,
                LocationId = e.LocationId,
                LocationName = e.LocationName,
                Longitude = e.Longitude,
                MaxAttends = e.MaxAttends,
                Owners = new List<LocationOwner>(),
                Start = e.Start,
                Title = e.Title,
                Website = e.Website,
                ZipCode = e.ZipCode,
                HasImage = e.HasImage
            };
        }

        public override void CreateDetailUrl( UrlHelper urlHelper )
        {
            DetailUrl = urlHelper.Link("DefaultApi", new { controller = "Event", id = EventId.ToString() });
        }

        public override void CreateImageUrl( UrlHelper urlHelper )
        { //urlHelper.Link("ControllerActionApi", new { controller = "app", action = "GetEventPicture", eventId = EventId.ToString() });
            if ( HasImage )
                ImageUrl = string.Format( "/api/App/GetEventPicture?eventId={0}", EventId );
            else
                ImageUrl = null;
        }
    }
}