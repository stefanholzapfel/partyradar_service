using System.Threading.Tasks;
using PartyService.ControllerModels.App;

namespace PartyService.Providers
{
    public class MockUserProvider:IUserProvider
    {
        public Task<UserDetail> GetUserDetailAsync( string userId )
        {
            return Task.FromResult( new UserDetail
            {
                Email = "andi.wand@fest.at",
                FirstName = "Andi",
                Id = userId,
                LastName = "Wand",
                UserName = "andiWand"
            } );
        }
    }
}