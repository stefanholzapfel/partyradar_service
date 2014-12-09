using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace PartyService.Models
{
    // You can add profile data for the user by adding more properties to your User class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

	public class ApplicationDbContext : IdentityDbContext<User>
    {
		public virtual DbSet<Location> Locations { get; set; }
		public virtual DbSet<Event> Events { get; set; }
		public virtual DbSet<Keyword> Keywords { get; set; }
		public DbSet<UserOnEvent> UserOnEvents { get; set; }
		public virtual DbSet<EventKeyword> EventKeywords { get; set; }
		public virtual DbSet<AdministrateLocation> AdministrateLocations { get; set; } 

        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

		public async Task<User> GetUserAsync( string userId )
		{
			 return await this.Users.SingleOrDefaultAsync( x => x.Id == userId );
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<EventKeyword>()
				.HasKey(x => new { x.EventId, x.KeywordId });

			modelBuilder.Entity<Event>()
				.HasMany( x => x.EventKeywords )
				.WithRequired()
				.HasForeignKey( x => x.EventId );

			modelBuilder.Entity<AdministrateLocation>()
				.HasKey( x => new { x.UserId, x.LocationId } );
			modelBuilder.Entity<User>()
				.HasMany( x => x.AdministrateLocations )
				.WithRequired()
				.HasForeignKey( x => x.UserId );
			modelBuilder.Entity<Location>()
				.HasMany( x => x.AdministrateLocations )
				.WithRequired()
				.HasForeignKey( x => x.LocationId );
				

			//modelBuilder.Entity<Event>()
			//	.Property( x => x.Image ).HasColumnType( "image" );
			
			base.OnModelCreating(modelBuilder);
		}
    }
}