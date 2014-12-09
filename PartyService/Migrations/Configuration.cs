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

			//var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
			//var roleName = "admin";
			//if (!roleManager.RoleExists(roleName))
			//	roleManager.Create(new IdentityRole(roleName));

			//roleName = "user";
			//if (!roleManager.RoleExists(roleName))
			//	roleManager.Create(new IdentityRole(roleName));

			//context.SaveChanges();
			base.Seed( context );
        }
    }
}
