using System;
using System.ComponentModel.DataAnnotations;

namespace PartyService.DatabaseModels
{
	public class UserOnEvent
	{
		[Key]
		public Guid Id { get; set; }
        public virtual string UserId { get; set; }
		public virtual User User{ get; set; }
	    public virtual Guid EventId { get; set; }
		public virtual Event Event { get; set; }
		public DateTime BeginTime { get; set; }
		public DateTime? EndTime { get; set; }
		public bool IsInactive { get; set; }
	}
}