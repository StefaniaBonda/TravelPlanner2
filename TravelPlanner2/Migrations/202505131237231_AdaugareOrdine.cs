namespace TravelPlanner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdaugareOrdine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConnectionBuildings", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.ConnectionCulinaries", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.ConnectionCulturals", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.ConnectionNatures", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConnectionNatures", "Order");
            DropColumn("dbo.ConnectionCulturals", "Order");
            DropColumn("dbo.ConnectionCulinaries", "Order");
            DropColumn("dbo.ConnectionBuildings", "Order");
        }
    }
}
