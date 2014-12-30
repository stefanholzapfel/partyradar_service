using System;

namespace PartyService.DatabaseModels
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