using System.Data.Entity;

namespace TravelPlanner2.Models
{
    public class MyDBContext : DbContext
    {
        public MyDBContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Nature> Natures { get; set; }
        public DbSet<Cultural> Culturals { get; set; }
        public DbSet<Culinary> Culinaries { get; set; }
        public DbSet<Buildings> Buildingss { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<ConnectionNature> ConnectionNatures { get; set; }
        public DbSet<ConnectionCultural> ConnectionCulturals { get; set; }
        public DbSet<ConnectionCulinary> ConnectionCulinaries { get; set; }
        public DbSet<ConnectionBuildings> ConnectionBuildingss { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favorite>()
                .HasRequired(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Favorite>()
                .HasRequired(f => f.Trip)
                .WithMany()
                .HasForeignKey(f => f.TripId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Feedback>()
                .HasRequired(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Feedback>()
                .HasRequired(f => f.Trip)
                .WithMany()
                .HasForeignKey(f => f.TripId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
