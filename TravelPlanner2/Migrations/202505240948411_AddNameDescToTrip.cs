namespace TravelPlanner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameDescToTrip : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trips", "Name", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Trips", "Description", c => c.String(maxLength: 500));
            AddColumn("dbo.Trips", "Published", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trips", "Published");
            DropColumn("dbo.Trips", "Description");
            DropColumn("dbo.Trips", "Name");
        }
    }
}
