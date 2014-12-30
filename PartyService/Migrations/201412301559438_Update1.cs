namespace PartyService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "TotalParticipants", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "TotalParticipants", c => c.Int(nullable: false));
        }
    }
}
