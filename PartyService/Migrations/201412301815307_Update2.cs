namespace PartyService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserOnEvents", "Event_Id", "dbo.Events");
            DropIndex("dbo.UserOnEvents", new[] { "Event_Id" });
            RenameColumn(table: "dbo.UserOnEvents", name: "Event_Id", newName: "EventId");
            RenameColumn(table: "dbo.UserOnEvents", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.UserOnEvents", name: "IX_User_Id", newName: "IX_UserId");
            AlterColumn("dbo.UserOnEvents", "EventId", c => c.Guid(nullable: false));
            CreateIndex("dbo.UserOnEvents", "EventId");
            AddForeignKey("dbo.UserOnEvents", "EventId", "dbo.Events", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserOnEvents", "EventId", "dbo.Events");
            DropIndex("dbo.UserOnEvents", new[] { "EventId" });
            AlterColumn("dbo.UserOnEvents", "EventId", c => c.Guid());
            RenameIndex(table: "dbo.UserOnEvents", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.UserOnEvents", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.UserOnEvents", name: "EventId", newName: "Event_Id");
            CreateIndex("dbo.UserOnEvents", "Event_Id");
            AddForeignKey("dbo.UserOnEvents", "Event_Id", "dbo.Events", "Id");
        }
    }
}
