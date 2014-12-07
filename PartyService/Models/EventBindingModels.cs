using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PartyService.Models
{
	public class AttendEventBindingModel
	{
		[Required, Display(Name="UserId")]
		public string UserId { get; set; }

		[Required, Display(Name="EventId")]
		public Guid EventId { get; set; }

	}
}