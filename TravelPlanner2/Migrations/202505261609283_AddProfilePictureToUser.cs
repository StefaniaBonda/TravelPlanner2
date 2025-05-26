
namespace TravelPlanner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfilePictureToUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "ProfilePicturePath", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "ProfilePicturePath", c => c.String());
        }
    }
}
