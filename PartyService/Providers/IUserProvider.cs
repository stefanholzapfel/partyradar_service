using System.Threading.Tasks;
using PartyService.ControllerModels.App;

namespace PartyService.Providers
{
    public interface IUserProvider
    {
        Task<UserDetail> GetUserDetailAsync( string userId );
    }
}
