using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<LoginCredential> LoginCredentials { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ApplicationRequest> ApplicationRequests { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LoginCredential>().ToTable("LoginCredentials");
            modelBuilder.Entity<RequestType>().ToTable("RequestTypes");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<ApplicationRequest>().ToTable("ApplicationRequests");

            modelBuilder.Entity<LoginCredential>()
                .HasOne(lc => lc.Role)
                .WithMany(r => r.LoginCredentials)
                .HasForeignKey(lc => lc.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationRequest>()
                .HasOne(ar => ar.Type)
                .WithMany(rt => rt.ApplicationRequests)
                .HasForeignKey(ar => ar.TypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
