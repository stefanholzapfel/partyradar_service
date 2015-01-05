﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PartyService.ControllerModels;
using PartyService.ControllerModels.App;
using PartyService.DatabaseModels;
using PartyService.Models;

namespace PartyService.Providers
{
    public interface IUserProvider
    {
        Task<UserDetail> GetAppUserDetailAsync( string userId );
        Task<WebUserDetail> GetWebUserDetailAsync( string userId );
        Task<List<WebUserDetail>> GetAllWebUserDetailsAsync( );
        Task<ResultSet<WebUserDetail>> AdminAddUserAsync( AddUser model );
        Task<Result> AdminChangeUserAsync( string userId, ChangeUser changeUser );
    }
}