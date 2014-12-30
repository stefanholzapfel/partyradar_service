using System.Threading.Tasks;
using PartyService.ControllerModels.App;
using PartyService.DatabaseModels;

namespace PartyService.Providers
{
    public class DbUserProvider:IUserProvider
    {
        public async Task<UserDetail> GetUserDetailAsync( string userId )
        {
            using ( var db = new ApplicationDbContext() )
            {
                var user = await db.GetUserAsync( userId );
                return new UserDetail
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = userId,
                    UserName = user.UserName
                };
            }
        }
    }
}