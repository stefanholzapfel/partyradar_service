using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PartyService.Providers
{
    public class MockPictureProvider:IPictureProvider
    {
        public System.Threading.Tasks.Task<ControllerModels.EventPicture> GetEventPictureAsync(Guid eventId)
        {
            throw new NotImplementedException();
        }
    }
}