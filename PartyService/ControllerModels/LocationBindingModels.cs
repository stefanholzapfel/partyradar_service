using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PartyService.Models;

namespace PartyService.ControllerModels
{
    public class CreateLocationBindingModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "PostalCode")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Latitude")]
        public double? Latitude { get; set; }

        [Display(Name = "Longitude")]
        public double? Longitude { get; set; }

        [Display(Name = "Total Participants")]
        public int TotalParticipants { get; set; }
    }

    public class UpdateLocationBindingModel : CreateLocationBindingModel
    {
        [Required]
        [Display(Name = "Id")]
        public Guid Id { get; set; }
    }
}