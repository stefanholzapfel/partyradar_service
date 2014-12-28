using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PartyService.Models;

namespace PartyService.ControllerModels
{
	public class ControllerEvent
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public string Description { get; set; }
		public Guid? ImageId { get; set; }
		public String NfcTagId { get; set; }
		public Guid LocationId { get; set; }
	    public int TotalParticipants { get; set; }

		public static ControllerEvent Convert( Event @event )
		{
			return new ControllerEvent
			{
				Description = @event.Description,
				EndTime = @event.EndTime,
				Id = @event.Id,
				LocationId = @event.Location.Id,
				Name = @event.Name,
				StartTime = @event.StartTime
			};
		}
	}
}