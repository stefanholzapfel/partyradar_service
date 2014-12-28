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
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public List<Keyword> Keywords { get; set; }
        public Guid LocationId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}