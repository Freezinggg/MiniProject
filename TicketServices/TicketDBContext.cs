using Microsoft.EntityFrameworkCore;
using TicketServices.Model;

namespace TicketServices
{
    public class TicketDBContext : DbContext
    {
        public TicketDBContext(DbContextOptions<TicketDBContext> options) : base(options)
        {

        }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Here you define the column mapping
            modelBuilder.Entity<Ticket>()
                .Property(f => f.TicketValidFrom)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<Ticket>()
                .Property(f => f.TicketValidTo)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<Ticket>()
                .Property(f => f.CreatedDate)
                .HasColumnType("timestamp without time zone");
        }
    }
}
