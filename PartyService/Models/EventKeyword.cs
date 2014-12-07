﻿using System;

namespace PartyService.Models
{
	public class EventKeyword
	{
		public virtual Guid EventId { get; set; }
		public virtual Guid KeywordId { get; set; }
		public virtual Keyword Keyword { get; set; }
		public virtual Event Event { get; set; }
	}
}