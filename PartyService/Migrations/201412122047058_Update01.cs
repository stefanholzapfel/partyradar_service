namespace PartyService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update01 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Locations", "Position");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Locations", "Position", c => c.Geometry());
        }
    }
}
