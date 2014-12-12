namespace PartyService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    
    public partial class Update02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "Position", c => c.Geography());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "Position");
        }
    }
}
