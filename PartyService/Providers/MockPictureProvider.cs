using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PartyService.ControllerModels;
using PartyService.Models;

namespace PartyService.Providers
{
    public class MockPictureProvider:IPictureProvider
    {
        public Task<ResultSet<EventPicture>> GetEventPictureAsync( Guid eventId )
        {
            throw new NotImplementedException();
        }
    }
}