using System;

namespace PartyService.DatabaseModels
{
	public class AdministrateLocation
	{
        public Guid LocationId { get; set; }
        public string UserId { get; set; }
		public virtual User User { get; set; }
		public virtual Location Location { get; set; }
        public virtual bool IsInactive { get; set; }
	}
}