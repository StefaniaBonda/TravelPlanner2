namespace TravelPlanner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPublishRequestedToTrip : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trips", "PublishRequested", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trips", "PublishRequested");
        }
    }
}
