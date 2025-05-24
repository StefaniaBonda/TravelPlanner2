namespace TravelPlanner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeNewFieldsOptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Trips", "Name", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Trips", "Name", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
