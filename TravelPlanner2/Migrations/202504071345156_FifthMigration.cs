namespace TravelPlanner2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FifthMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Buildings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Price = c.Double(nullable: false),
                        Country = c.String(nullable: false),
                        City = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConnectionBuildings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TripId = c.Int(nullable: false),
                        BuildingsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Buildings", t => t.BuildingsId, cascadeDelete: true)
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .Index(t => t.TripId)
                .Index(t => t.BuildingsId);
            
            CreateTable(
                "dbo.Trips",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        price = c.Double(nullable: false),
                        timeRange = c.Double(nullable: false),
                        kmRange = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConnectionCulinaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TripId = c.Int(nullable: false),
                        CulinaryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Culinaries", t => t.CulinaryId, cascadeDelete: true)
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .Index(t => t.TripId)
                .Index(t => t.CulinaryId);
            
            CreateTable(
                "dbo.Culinaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Price = c.Double(nullable: false),
                        Country = c.String(nullable: false),
                        City = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConnectionCulturals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TripId = c.Int(nullable: false),
                        CulturalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Culturals", t => t.CulturalId, cascadeDelete: true)
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .Index(t => t.TripId)
                .Index(t => t.CulturalId);
            
            CreateTable(
                "dbo.Culturals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Price = c.Double(nullable: false),
                        Country = c.String(nullable: false),
                        City = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConnectionNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TripId = c.Int(nullable: false),
                        NatureId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Natures", t => t.NatureId, cascadeDelete: true)
                .ForeignKey("dbo.Trips", t => t.TripId, cascadeDelete: true)
                .Index(t => t.TripId)
                .Index(t => t.NatureId);
            
            CreateTable(
                "dbo.Natures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Price = c.Double(nullable: false),
                        Country = c.String(nullable: false),
                        City = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Favorites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trips", t => t.TripId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId, unique: true, name: "IX_User_Trip")
                .Index(t => t.TripId);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TripId = c.Int(nullable: false),
                        Comment = c.String(),
                        Rating = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trips", t => t.TripId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.TripId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Feedbacks", "UserId", "dbo.Users");
            DropForeignKey("dbo.Feedbacks", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Favorites", "UserId", "dbo.Users");
            DropForeignKey("dbo.Favorites", "TripId", "dbo.Trips");
            DropForeignKey("dbo.ConnectionNatures", "TripId", "dbo.Trips");
            DropForeignKey("dbo.ConnectionNatures", "NatureId", "dbo.Natures");
            DropForeignKey("dbo.ConnectionCulturals", "TripId", "dbo.Trips");
            DropForeignKey("dbo.ConnectionCulturals", "CulturalId", "dbo.Culturals");
            DropForeignKey("dbo.ConnectionCulinaries", "TripId", "dbo.Trips");
            DropForeignKey("dbo.ConnectionCulinaries", "CulinaryId", "dbo.Culinaries");
            DropForeignKey("dbo.ConnectionBuildings", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Trips", "UserId", "dbo.Users");
            DropForeignKey("dbo.ConnectionBuildings", "BuildingsId", "dbo.Buildings");
            DropIndex("dbo.Feedbacks", new[] { "TripId" });
            DropIndex("dbo.Feedbacks", new[] { "UserId" });
            DropIndex("dbo.Favorites", new[] { "TripId" });
            DropIndex("dbo.Favorites", "IX_User_Trip");
            DropIndex("dbo.ConnectionNatures", new[] { "NatureId" });
            DropIndex("dbo.ConnectionNatures", new[] { "TripId" });
            DropIndex("dbo.ConnectionCulturals", new[] { "CulturalId" });
            DropIndex("dbo.ConnectionCulturals", new[] { "TripId" });
            DropIndex("dbo.ConnectionCulinaries", new[] { "CulinaryId" });
            DropIndex("dbo.ConnectionCulinaries", new[] { "TripId" });
            DropIndex("dbo.Trips", new[] { "UserId" });
            DropIndex("dbo.ConnectionBuildings", new[] { "BuildingsId" });
            DropIndex("dbo.ConnectionBuildings", new[] { "TripId" });
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Favorites");
            DropTable("dbo.Natures");
            DropTable("dbo.ConnectionNatures");
            DropTable("dbo.Culturals");
            DropTable("dbo.ConnectionCulturals");
            DropTable("dbo.Culinaries");
            DropTable("dbo.ConnectionCulinaries");
            DropTable("dbo.Users");
            DropTable("dbo.Trips");
            DropTable("dbo.ConnectionBuildings");
            DropTable("dbo.Buildings");
        }
    }
}
