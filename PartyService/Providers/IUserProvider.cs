using System.Collections.Generic;
using System.Threading.Tasks;
using PartyService.ControllerModels;
using PartyService.ControllerModels.App;
using PartyService.Models;

namespace PartyService.Providers
{
    public interface IUserProvider
    {
        Task<AppUserDetail> GetAppUserDetailAsync( string userId );
        Task<WebUserDetail> GetWebUserDetailAsync( string userId );
        Task<List<WebUserDetail>> GetAllWebUserDetailsAsync( );
        Task<ResultSet<WebUserDetail>> AdminAddUserAsync( AddUser model );
        Task<Result> AdminChangeUserAsync( string userId, ChangeUser changeUser );
        Task<bool> UserExistAsync( string userId );
        Task<Result> RemoveUserAsync( string userId );
    }
}
