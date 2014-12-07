using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
namespace PartyService.Models
{
	public class User : IdentityUser
	{
		[Required]
		public virtual DateTime BirthDate { get; set; }

		[Required]
		public virtual GenderType Gender { get; set; }
		
		[StringLength(Config.DEFAULT_STRING_LENGTH_I)]
		public virtual string FirstName { get; set; }
		
		[StringLength(Config.DEFAULT_STRING_LENGTH_I)]
		public virtual string LastName { get; set; }
		
		public bool IsInactive { get; set; }
		public virtual ICollection<AdministrateLocation> AdministrateLocations { get; set; } 
		

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
			// Add custom user claims here
			return userIdentity;
		}
	}
}