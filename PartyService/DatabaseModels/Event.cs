using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyService.Models
{
	public class Event
	{
		public Guid Id { get; set; }

		[StringLength(Config.DEFAULT_STRING_LENGTH_I)]
		public string Name { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }

		[StringLength(Config.DEFAULT_STRING_LENGTH_I)]
		public string NfcTagId { get; set; }
		public virtual Location Location { get; set; }
		public List<EventKeyword> EventKeywords { get; set; }
		public bool IsInactive { get; set; }

		[Column(TypeName = "ntext")]
		public string Description { get; set; }

		[Column(TypeName = "image")]
		public byte[] Image { get; set; }

        public int TotalParticipants { get; set; }
	}
}