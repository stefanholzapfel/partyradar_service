using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PartyService.Models;

namespace PartyService.ControllerModels
{
    public class ControllerLocation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string Place { get; set; }
        public int TotalParticipants { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public ICollection<User> Administrators { get; set; }
        public bool IsInactive { get; set; }

        public static ControllerLocation Convert( Location location )
        {
            return new ControllerLocation
            {
                Id = location.Id,
                Latitude = location.Position.Latitude,
                Longitude = location.Position.Longitude,
                Name = location.Name,
                Place = location.Place,
                PostalCode = location.PostalCode,
                Street = location.Street,
                IsInactive = location.IsInactive,
                TotalParticipants = location.TotalParticipants,
                Administrators = location.AdministrateLocations.Select(x => new User
                    {
                        Id = x.UserId,
                        LoginName = x.User.UserName
                }).ToArray()
            };
        }
    }
}