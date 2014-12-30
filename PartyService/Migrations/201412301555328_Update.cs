namespace PartyService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "StreetAddition", c => c.String(maxLength: 256));
            AddColumn("dbo.Locations", "City", c => c.String(maxLength: 256));
            AddColumn("dbo.Locations", "Country", c => c.String(maxLength: 256));
            AddColumn("dbo.Locations", "Website", c => c.String(maxLength: 1024));
            AddColumn("dbo.Events", "Website", c => c.String(maxLength: 1024));
            DropColumn("dbo.Locations", "Place");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Locations", "Place", c => c.String(maxLength: 256));
            DropColumn("dbo.Events", "Website");
            DropColumn("dbo.Locations", "Website");
            DropColumn("dbo.Locations", "Country");
            DropColumn("dbo.Locations", "City");
            DropColumn("dbo.Locations", "StreetAddition");
        }
    }
}
