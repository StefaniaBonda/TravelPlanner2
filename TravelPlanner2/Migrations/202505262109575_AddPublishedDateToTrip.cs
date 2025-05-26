namespace TravelPlanner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPublishedDateToTrip : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trips", "PublishedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trips", "PublishedDate");
        }
    }
}
