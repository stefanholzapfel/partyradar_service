using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PartyService.Models;

namespace PartyService.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PartyService.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PartyService.Models.ApplicationDbContext context)
        {
			//  This method will be called after migrating to the latest version.


			
			var manager = new UserManager<User>(new UserStore<User>(context));
			var adminUser = new User
			{
				EmailConfirmed = true,
				Email = "party0n.adm1n@gmail.com",
				BirthDate = DateTime.Parse("1971-01-01"),
				Gender = GenderType.Male,
				UserName = "admin",
				FirstName = "Admin",
				LastName = "Strator"
			};

	        
			
			manager.Create(adminUser, "PartyOnAdmin1!");
	        context.SaveChanges();

			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
			var roleName = "admin";
			if (!roleManager.RoleExists(roleName))
				roleManager.Create(new IdentityRole(roleName));

			roleName = "user";
			if (!roleManager.RoleExists(roleName))
				roleManager.Create(new IdentityRole(roleName));
			base.Seed( context );
        }
    }
}
