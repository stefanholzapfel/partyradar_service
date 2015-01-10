using System;
using System.Data.Entity;
using System.Threading.Tasks;
using PartyService.ControllerModels;
using PartyService.DatabaseModels;
using PartyService.Models;

namespace PartyService.Providers
{
    public class DbPictureProvider:IPictureProvider
    {
        public async Task<ResultSet<EventPicture>> GetEventPictureAsync( Guid eventId )
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var @event = await db.Events.SingleOrDefaultAsync(x => x.Id == eventId);
                    if (@event == null)
                        return new ResultSet<EventPicture>(false, "Image not found for given Id!");

                    return new ResultSet<EventPicture>(true)
                    {
                        Result = new EventPicture
                        {
                            EventId = eventId,
                            Image = @event.Image
                        }
                    };
                }
            }
            catch ( Exception exc )
            {
                return new ResultSet<EventPicture>( false, exc.Message );
            }
        }
    }
}