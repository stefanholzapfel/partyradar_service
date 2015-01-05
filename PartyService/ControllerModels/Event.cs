using System.Web.Http.Routing;
using PartyService.Areas.HelpPage.ModelDescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PartyService.ControllerModels.App
{
    [ModelName("AppEvent")]
    public class Event
    {
        public Guid EventId { get; set; }
        public string Title { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public List<Keyword> Keywords { get; set; }
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string DetailUrl { get; set; }
        public double? Distance { get; set; }
        public int AttendeeCount { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Country { get; set; }
        public int? MaxAttends { get; set; }
        public string Adress { get; set; }
        public string AdressAdditions { get; set; }
        public string ImageUrl { get; set; }

        public void CreateDetailUrl(UrlHelper urlHelper)
        {
            DetailUrl = urlHelper.Link("ControllerActionApi", new { controller = "app", action = "GetEvent", eventId = EventId.ToString() });
        }

        public void CreateImageUrl(UrlHelper urlHelper)
        {
            ImageUrl = urlHelper.Link("ControllerActionApi", new { controller = "app", action = "GetEventPicture", eventId = EventId.ToString() });
        }

        public void CreateUrls( UrlHelper urlHelper )
        {
            CreateDetailUrl( urlHelper );
            CreateImageUrl( urlHelper );
        }
    }
}