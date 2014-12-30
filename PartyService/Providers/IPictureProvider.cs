using PartyService.ControllerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyService.Providers
{
    public interface IPictureProvider
    {
        Task<EventPicture> GetEventPictureAsync(Guid eventId);
    }
}
