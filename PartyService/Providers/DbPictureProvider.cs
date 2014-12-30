using System;
using System.Data.Entity;
using System.Threading.Tasks;
using PartyService.ControllerModels;
using PartyService.DatabaseModels;

namespace PartyService.Providers
{
    public class DbPictureProvider:IPictureProvider
    {
        public async Task<ControllerModels.EventPicture> GetEventPictureAsync(Guid eventId)
        {
            using ( var db = new ApplicationDbContext() )
            {
                var @event = await db.Events.SingleOrDefaultAsync( x => x.Id == eventId );
                return new EventPicture
                {
                    EventId = eventId,
                    Image = @event.Image
                };
            }
        }
    }
}