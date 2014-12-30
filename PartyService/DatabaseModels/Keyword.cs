using System;
using System.ComponentModel.DataAnnotations;

namespace PartyService.DatabaseModels
{
	public class Keyword
	{
		[Key]
		public Guid Id { get; set; }

		[StringLength(Config.DEFAULT_STRING_LENGTH_I)]
		public string Label { get; set; }
		public bool IsInactive { get; set; }
	}
}