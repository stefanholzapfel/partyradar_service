using PartyService.Areas.HelpPage.ModelDescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PartyService.ControllerModels
{
    [ModelName("RequestKeyword")]
    public class Keyword
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
    }
}