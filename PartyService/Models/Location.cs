using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace PartyService.Models
{
	public class Location
	{
		public Guid Id { get; set; }

		[StringLength(Config.DEFAULT_STRING_LENGTH_I)]
		public string Name { get; set; }
		public DbGeometry Position { get; set; }

		[StringLength(Config.DEFAULT_STRING_LENGTH_I)]
		public string Street { get; set; }

		[StringLength(Config.DEFAULT_STRING_LENGTH_I)]
		public string PostalCode { get; set; }

		[StringLength(Config.DEFAULT_STRING_LENGTH_I)]
		public string Place { get; set; }
		public int TotalParticipants { get; set; }
		public virtual ICollection<AdministrateLocation> AdministrateLocations { get; set; }
		public bool IsInactive { get; set; }
	}
}