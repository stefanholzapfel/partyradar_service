using System;
using System.Collections.Generic;
using System.Linq;
using PartyService.DatabaseModels;

namespace PartyService.ControllerModels
{
    public class LocationDetail
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string AddressAdditions { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public ICollection<LocationOwner> Owners { get; set; }
        public int? MaxAttends { get; set; }
    }
}