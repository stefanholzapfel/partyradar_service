using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PartyService.DatabaseModels;
using PartyService.Models;

namespace PartyService.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
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
