using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;

namespace PartyService.DatabaseModels
{
	public class Location
	{
		public Guid Id { get; set; }

		[StringLength(Config.DEFAULT_STRING_LENGTH_I)]
		public string Name { get; set; }
		public DbGeography Position { get; set; }

		[StringLength(Config.DEFAULT_STRING_LENGTH_I)]
		public string Street { get; set; }

        [StringLength(Config.DEFAULT_STRING_LENGTH_I)]
	    public string StreetAddition { get; set; }

		[StringLength(Config.DEFAULT_STRING_LENGTH_I)]
		public string PostalCode { get; set; }

		[StringLength(Config.DEFAULT_STRING_LENGTH_I)]
		public string City { get; set; }

        [StringLength(Config.DEFAULT_STRING_LENGTH_I)]
	    public string Country { get; set; }

		public int TotalParticipants { get; set; }

		public virtual ICollection<AdministrateLocation> AdministrateLocations { get; set; }

		public bool IsInactive { get; set; }

        [StringLength(Config.LONG_STRING_LENGTH_I)]
        public string Website { get; set; }
	}
}