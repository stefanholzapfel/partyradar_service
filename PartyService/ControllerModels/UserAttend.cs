using System;

namespace PartyService.ControllerModels
{
    public class UserAttend
    {
        public string UserId { get; set; }
        public Guid EventId { get; set; }
        public DateTime Start { get; set; }
    }
}