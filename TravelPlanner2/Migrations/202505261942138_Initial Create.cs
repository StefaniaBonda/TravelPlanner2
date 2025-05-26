namespace TravelPlanner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trips", "PublishRequested", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "ProfilePicturePath", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ProfilePicturePath");
            DropColumn("dbo.Trips", "PublishRequested");
        }
    }
}
