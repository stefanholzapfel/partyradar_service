using System;
using System.ComponentModel.DataAnnotations;

namespace PartyService.Models
{
	public class UserOnEvent
	{
		[Key]
		public Guid Id { get; set; }
		public User User{ get; set; }
		public Event Event { get; set; }
		public DateTime BeginTime { get; set; }
		public DateTime? EndTime { get; set; }
		public bool IsInactive { get; set; }
	}
}