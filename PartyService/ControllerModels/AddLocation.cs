﻿using System.ComponentModel.DataAnnotations;

namespace PartyService.ControllerModels
{
    public class AddLocation
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "AddressAdditions")]
        public string AddressAdditions { get; set; }

        [Required]
        [Display(Name = "ZipCode")]
        public string ZipCode { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Latitude")]
        public double? Latitude { get; set; }

        [Display(Name = "Longitude")]
        public double? Longitude { get; set; }

        [Display(Name = "MaxAttends")]
        public int? MaxAttends { get; set; }

        [Display(Name = "Website")]
        public string Website { get; set; }
    }
}