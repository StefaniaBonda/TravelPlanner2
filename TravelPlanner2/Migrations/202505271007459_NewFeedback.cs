namespace TravelPlanner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewFeedback : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Favorites", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Favorites", "UserId", "dbo.Users");
            DropForeignKey("dbo.Feedbacks", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Feedbacks", "UserId", "dbo.Users");
            DropIndex("dbo.Favorites", "IX_User_Trip");
            DropIndex("dbo.Favorites", new[] { "TripId" });
            DropIndex("dbo.Feedbacks", new[] { "UserId" });
            DropIndex("dbo.Feedbacks", new[] { "TripId" });
            AddColumn("dbo.Feedbacks", "Name", c => c.String(nullable: false));
            AddColumn("dbo.Feedbacks", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Feedbacks", "Message", c => c.String(nullable: false));
            AddColumn("dbo.Feedbacks", "SubmittedAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.Feedbacks", "UserId");
            DropColumn("dbo.Feedbacks", "TripId");
            DropColumn("dbo.Feedbacks", "Comment");
            DropColumn("dbo.Feedbacks", "Rating");
            DropColumn("dbo.Feedbacks", "CreatedAt");
            DropTable("dbo.Favorites");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Favorites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Feedbacks", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Feedbacks", "Rating", c => c.Int(nullable: false));
            AddColumn("dbo.Feedbacks", "Comment", c => c.String());
            AddColumn("dbo.Feedbacks", "TripId", c => c.Int(nullable: false));
            AddColumn("dbo.Feedbacks", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.Feedbacks", "SubmittedAt");
            DropColumn("dbo.Feedbacks", "Message");
            DropColumn("dbo.Feedbacks", "Email");
            DropColumn("dbo.Feedbacks", "Name");
            CreateIndex("dbo.Feedbacks", "TripId");
            CreateIndex("dbo.Feedbacks", "UserId");
            CreateIndex("dbo.Favorites", "TripId");
            CreateIndex("dbo.Favorites", "UserId", unique: true, name: "IX_User_Trip");
            AddForeignKey("dbo.Feedbacks", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Feedbacks", "TripId", "dbo.Trips", "Id");
            AddForeignKey("dbo.Favorites", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Favorites", "TripId", "dbo.Trips", "Id");
        }
    }
}
