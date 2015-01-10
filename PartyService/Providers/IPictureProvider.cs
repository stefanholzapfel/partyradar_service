using System;
using System.Threading.Tasks;
using PartyService.ControllerModels;
using PartyService.Models;

namespace PartyService.Providers
{
    public interface IPictureProvider
    {
        Task<ResultSet<EventPicture>> GetEventPictureAsync( Guid eventId );
    }
}
