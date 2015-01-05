namespace PartyService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Locations", "TotalParticipants", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Locations", "TotalParticipants", c => c.Int(nullable: false));
        }
    }
}
