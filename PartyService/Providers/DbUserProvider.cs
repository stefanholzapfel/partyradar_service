using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PartyService.ControllerModels;
using PartyService.ControllerModels.App;
using PartyService.DatabaseModels;
using PartyService.Models;
using WebGrease.Css.Extensions;
using User = PartyService.DatabaseModels.User;

namespace PartyService.Providers
{
    public class DbUserProvider:IUserProvider
    {
        public ApplicationUserManager UserManager { get; set; }
        public async Task<UserDetail> GetAppUserDetailAsync( string userId )
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

        public async Task<WebUserDetail> GetWebUserDetailAsync( string userId )
        {
            using (var db = new ApplicationDbContext())
            {
                var user = await db.GetUserAsync(userId);
                return new WebUserDetail
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = userId,
                    UserName = user.UserName,
                    IsAdmin = await UserManager.IsInRoleAsync( userId,Roles.Admin )
                };
            }
        }

        public async Task<List<WebUserDetail>> GetAllWebUserDetailsAsync( )
        {
            using (var db = new ApplicationDbContext())
            {
                var users = ( await db.Users.ToArrayAsync() )
                    .Select( user => new WebUserDetail
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Id = user.Id,
                        UserName = user.UserName,
                        IsAdmin = UserManager.IsInRole( user.Id,Roles.Admin )
                    }
                ).ToList();
                return users;
            }
        }

        public async Task<ResultSet<WebUserDetail>> AdminAddUserAsync( AddUser model )
        {
            var user = new User()
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                BirthDate = model.BirthDate,
                Gender = model.Gender
            };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if ( !result.Succeeded )
                return new ResultSet<WebUserDetail>( false );


            var createdUser = await UserManager.FindByNameAsync(model.UserName);

            string[] roles = model.IsAdmin ? new[] { Roles.User, Roles.Admin } : new[] { Roles.User };
            var addRolesResult = await UserManager.AddToRolesAsync(createdUser.Id, roles);

            if ( !addRolesResult.Succeeded )
                return new ResultSet<WebUserDetail>( false, "Could not add roles to user: " + string.Join( ", ", roles ) );

            return new ResultSet<WebUserDetail>( true )
            {
                Result = new WebUserDetail
                {
                    Email = createdUser.Email,
                    FirstName = createdUser.FirstName,
                    Id = createdUser.Id,
                    IsAdmin = model.IsAdmin,
                    LastName = createdUser.LastName,
                    UserName = createdUser.UserName
                }
            };
        }

        public async Task<Result> AdminChangeUserAsync( string userId, ChangeUser changeUser )
        {
            using ( var db = new ApplicationDbContext() )
            {
                var user = await db.Users.FirstOrDefaultAsync( x => x.Id == userId );
                
                if ( user == null )
                    return new ResultSet<WebUserDetail>( false, "Could not find user with id: " + userId );

                if ( changeUser.BirthDate.HasValue )
                    user.BirthDate = changeUser.BirthDate.Value;

                if ( changeUser.Gender.HasValue )
                    user.Gender = changeUser.Gender.Value;

                if ( !string.IsNullOrEmpty( changeUser.Email ) )
                    user.Email = changeUser.Email;

                if ( !string.IsNullOrEmpty( changeUser.FirstName ) )
                    user.FirstName = changeUser.FirstName;

                if ( !string.IsNullOrEmpty( changeUser.LastName ) )
                    user.LastName = changeUser.LastName;

                db.Users.Attach( user );
                await db.SaveChangesAsync();

                if ( changeUser.IsAdmin.HasValue )
                {
                    var isInAdminRole = await UserManager.IsInRoleAsync(userId, Roles.Admin);
                    if ( isInAdminRole != changeUser.IsAdmin )
                    {
                        if ( changeUser.IsAdmin.Value )
                            await UserManager.AddToRoleAsync(userId, Roles.Admin);
                        else
                            await UserManager.RemoveFromRoleAsync(userId, Roles.Admin);
                    }
                    await UpdateUserRolesWithClaims( userId );
                }
                    
            }
            return new Result( true );
        }

        public async Task<bool> UserExistAsync( string userId )
        {
            using ( var db = new ApplicationDbContext() )
            {
                return await db.Users.AnyAsync( x => x.Id == userId );
            }
        }

        private async Task UpdateUserRolesWithClaims( string userId )
        {
            var user = await UserManager.FindByIdAsync( userId );
            var userRoleIds = user.Roles.Select( x => x.RoleId ).ToArray();
            var claimValues = user.Claims.Where( x => x.ClaimType == ClaimTypes.Role ).Select( x=>x.ClaimValue ).ToArray();

            using ( var db = new ApplicationDbContext() )
            {
                var allRoles = db.Roles.ToArray();

                foreach ( var roleId in userRoleIds )
                {
                    if ( !claimValues.Contains( roleId ))
                        user.Claims.Add( new IdentityUserClaim{ClaimType = ClaimTypes.Role,ClaimValue = allRoles.Single(x=>x.Id == roleId).Name,UserId = userId} );
                }
                await UserManager.UpdateAsync( user );
            }
        }
    }
}