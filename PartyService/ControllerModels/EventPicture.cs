using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PartyService.ControllerModels
{
    public class EventPicture
    {
        public Guid EventId { get; set; }
        public byte[] Image { get; set; }
    }
}