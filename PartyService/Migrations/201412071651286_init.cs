namespace PartyService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdministrateLocations",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LocationId = c.Guid(nullable: false),
                        IsInactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.LocationId })
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 256),
                        Position = c.Geometry(),
                        Street = c.String(maxLength: 256),
                        PostalCode = c.String(maxLength: 256),
                        Place = c.String(maxLength: 256),
                        TotalParticipants = c.Int(nullable: false),
                        IsInactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        BirthDate = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                        FirstName = c.String(maxLength: 256),
                        LastName = c.String(maxLength: 256),
                        IsInactive = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.EventKeywords",
                c => new
                    {
                        EventId = c.Guid(nullable: false),
                        KeywordId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.EventId, t.KeywordId })
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.Keywords", t => t.KeywordId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.KeywordId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 256),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        NfcTagId = c.String(maxLength: 256),
                        IsInactive = c.Boolean(nullable: false),
                        Description = c.String(storeType: "ntext"),
                        Image = c.Binary(storeType: "image"),
                        Location_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .Index(t => t.Location_Id);
            
            CreateTable(
                "dbo.Keywords",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Label = c.String(maxLength: 256),
                        IsInactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserOnEvents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BeginTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(),
                        IsInactive = c.Boolean(nullable: false),
                        Event_Id = c.Guid(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Event_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserOnEvents", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserOnEvents", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.EventKeywords", "KeywordId", "dbo.Keywords");
            DropForeignKey("dbo.Events", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.EventKeywords", "EventId", "dbo.Events");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AdministrateLocations", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AdministrateLocations", "LocationId", "dbo.Locations");
            DropIndex("dbo.UserOnEvents", new[] { "User_Id" });
            DropIndex("dbo.UserOnEvents", new[] { "Event_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Events", new[] { "Location_Id" });
            DropIndex("dbo.EventKeywords", new[] { "KeywordId" });
            DropIndex("dbo.EventKeywords", new[] { "EventId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AdministrateLocations", new[] { "LocationId" });
            DropIndex("dbo.AdministrateLocations", new[] { "UserId" });
            DropTable("dbo.UserOnEvents");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Keywords");
            DropTable("dbo.Events");
            DropTable("dbo.EventKeywords");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Locations");
            DropTable("dbo.AdministrateLocations");
        }
    }
}
