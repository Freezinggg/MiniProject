using FacilityServices.Model;
using Microsoft.EntityFrameworkCore;

namespace FacilityServices
{
    public class FacilityDbContext : DbContext
    {
        public FacilityDbContext(DbContextOptions<FacilityDbContext> options) : base(options)
        {

        }

        public DbSet<Facility> Facilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Here you define the column mapping
            modelBuilder.Entity<Facility>()
                .Property(f => f.CreatedDate)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<Facility>()
                .Property(f => f.UpdatedDate)
                .HasColumnType("timestamp without time zone");
        }
    }
}
