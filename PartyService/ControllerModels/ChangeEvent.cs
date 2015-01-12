using System;

namespace PartyService.ControllerModels
{
    public class ControllerChangeEvent
    {
        public string Title { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public Guid? LocationId { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public int? MaxAttends { get; set; }
        public string Image { get; set; }
        public Guid[] KeywordIds { get; set; }
    }
    public class ChangeEvent: ControllerChangeEvent
    {
        public Guid EventId { get; set; }
    }
}