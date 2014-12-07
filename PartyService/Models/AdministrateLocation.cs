using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PartyService.Models
{
	public class AdministrateLocation
	{
		public Guid LocationId { get; set; }
		public string UserId { get; set; }

		public User User { get; set; }
		public Location Location { get; set; }
		public bool IsInactive { get; set; }
	}
}