using PartyService.ControllerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyService.Providers
{
    interface IPictureProvider
    {
        EventPicture GetEventPicture(string userId, Guid eventId);
    }
}
