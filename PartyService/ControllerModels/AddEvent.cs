using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PartyService.Areas.HelpPage.ModelDescriptions;

namespace PartyService.ControllerModels
{
    [ModelName("AddEvent")]
    public class AddEvent
    {
        [Required]
        public string Title { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public Guid[] KeywordIds { get; set; }
        [Required]
        public Guid LocationId { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public int? MaxAttends { get; set; }
        public byte[] Image { get; set; }
    }
}