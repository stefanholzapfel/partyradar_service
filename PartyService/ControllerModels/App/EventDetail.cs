using PartyService.Areas.HelpPage.ModelDescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PartyService.ControllerModels.App
{
    [ModelName("AppEventDetail")]
    public class EventDetail:Event
    {
        public string Description { get; set; }
        public string Website { get; set; }
        public string LocationName { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public int AttendeeCount { get; set; }
    }
}