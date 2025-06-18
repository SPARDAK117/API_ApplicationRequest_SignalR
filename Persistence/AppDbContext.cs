using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ApplicationRequest> ApplicationRequests { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationRequest>(entity =>
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<ApplicationRequest>()
                    .HasOne(ar => ar.Type)
                    .WithMany(rt => rt.ApplicationRequests)
                    .HasForeignKey(ar => ar.TypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
