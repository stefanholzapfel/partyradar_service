using System;

namespace PartyService.DatabaseModels
{
	public class AdministrateLocation
	{
        public virtual Guid LocationId { get; set; }
        public virtual string UserId { get; set; }
		public virtual User User { get; set; }
		public virtual Location Location { get; set; }
        public virtual bool IsInactive { get; set; }
	}
}