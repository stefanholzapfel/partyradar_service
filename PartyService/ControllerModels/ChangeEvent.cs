using System;

namespace PartyService.ControllerModels
{
    public class ChangeEvent
    {
        public string Title { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public Guid? LocationId { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public int? MaxAttends { get; set; }
    }
}