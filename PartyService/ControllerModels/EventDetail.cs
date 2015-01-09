using System.Collections.Generic;

namespace PartyService.ControllerModels
{
    public class EventDetail : Event
    {
        public ICollection<LocationOwner> Owners { get; set; }
    }
}