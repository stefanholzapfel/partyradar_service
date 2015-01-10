using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Http.Routing;
using PartyService.Areas.HelpPage.ModelDescriptions;

namespace PartyService.ControllerModels
{
    [ModelName("AppEvent")]
    [DataContract]
    public class Event
    {
        [DataMember]
        public Guid EventId { get; set; }
        
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public DateTime? Start { get; set; }

        [DataMember]
        public DateTime? End { get; set; }

        [DataMember]
        public List<Keyword> Keywords { get; set; }

        [DataMember]
        public Guid LocationId { get; set; }

        [DataMember]
        public string LocationName { get; set; }

        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public double Longitude { get; set; }

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public string DetailUrl { get; set; }

        [DataMember]
        public double? Distance { get; set; }

        [DataMember]
        public int AttendeeCount { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Website { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public int? MaxAttends { get; set; }

        [DataMember]
        public string Adress { get; set; }

        [DataMember]
        public string AdressAdditions { get; set; }

        [DataMember]
        public string ImageUrl { get; set; }

        public bool HasImage { get; set; }

        public virtual void CreateDetailUrl(UrlHelper urlHelper)
        {
            DetailUrl = urlHelper.Link("ControllerActionApi", new { controller = "app", action = "GetEvent", eventId = EventId.ToString() });
        }

        public virtual void CreateImageUrl(UrlHelper urlHelper)
        {
            if ( HasImage )
                ImageUrl = urlHelper.Link( "ControllerActionApi", new { controller = "app", action = "GetEventPicture", eventId = EventId.ToString() } );
            else
                ImageUrl = null;
        }

        public void CreateUrls( UrlHelper urlHelper )
        {
            CreateDetailUrl( urlHelper );
            CreateImageUrl( urlHelper );
        }
    }
}