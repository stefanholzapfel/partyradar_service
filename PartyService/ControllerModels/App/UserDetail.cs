using System;
using PartyService.Models;

namespace PartyService.ControllerModels.App
{
    public abstract class UserDetail
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class AppUserDetail:UserDetail
    {
        public Guid? AttendEventId { get; set; }
    }

    public class WebUserDetail : UserDetail
    {
        public bool IsAdmin { get; set; }
        public GenderType Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public static WebUserDetail ParseTo( UserDetail userDetail )
        {
            return new WebUserDetail
            {
                Email = userDetail.Email,
                FirstName = userDetail.FirstName,
                Id = userDetail.Id,
                IsAdmin = false,
                LastName = userDetail.LastName,
                UserName = userDetail.UserName,
                Gender = GenderType.Unknown
            };
        }
    }
}